using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder(-100)]
public class LevelController : MonoBehaviour
{
    [System.Serializable]
    public class Transition
    {
        public Bounds bounds;
        public Vector3 spawnOffset;
        public float spawnInputLockDuration = .5f;
        public TransitionDirection direction;
        public string scene;
    }

    public enum TransitionDirection
    {
        Left = -1,
        Right = 1,
        Up = 2,
        Down = -2
    }

    public FadeToBlack fader;
    public Player playerPrefab;

    public Player Player { get; private set; }

    public TransitionDirection defaultTransitionDirection;
    public List<Transition> transitions;

    private LayerMask playerLayerMask = 0;
    private bool playerEntryInProgress = false;

    private Transition _enterTransition;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        SaveLoadManager.Instance.Init();
        InventoryManager.Instance.Init();
        
        Player = Instantiate(playerPrefab);
        playerLayerMask |= (1 << Player.gameObject.layer);

        SetPlayerEntryPosition();
        Player.Health.onDeath += Respawn;

        UnlocksManager.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerEntryInProgress)
        {
            return;
        }

        foreach(var t in transitions)
        {
            Collider2D other = Physics2D.OverlapBox(t.bounds.center, t.bounds.size, 0, playerLayerMask);
            if (other)
            {
                StartCoroutine(SwitchSceneCrt(t));
                break;
            }
        }
    }

    private void Respawn()
    {
        StartCoroutine(RespawnCrt());
    }

    private IEnumerator RespawnCrt()
    {
        yield return StartCoroutine(fader.FadeOut());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetPlayerEntryPosition()
    {
        int lastExitDirection = SaveLoadManager.Instance.Data.LastExitDirection;
        TransitionDirection direction = lastExitDirection != 0
            ? (TransitionDirection) (-lastExitDirection)
            : defaultTransitionDirection;

        _enterTransition = transitions.Find(t => t.direction == direction);
        if (_enterTransition == null && direction != defaultTransitionDirection)
        {
            _enterTransition = transitions.Find(t => t.direction == defaultTransitionDirection);
        }

        if (_enterTransition != null)
        {
            StartCoroutine(EnterPlayerCrt(_enterTransition));
        }
    }

    private IEnumerator EnterPlayerCrt(Transition transition)
    {
        Player.transform.position = transition.bounds.center + transition.spawnOffset;
        playerEntryInProgress = true;

        if (Mathf.Abs((int)transition.direction) == 1)
        {
            Player.Controller.SetHorizontalMovement(-(int)transition.direction * Player.Movement.movementSpeed);
            Player.Controller.LockInput = true;

            yield return new WaitForSeconds(transition.spawnInputLockDuration);
        }
        else if(transition.direction == TransitionDirection.Down)
        {

            Vector2 dashDirection = new Vector2(/*Random.value < .5f ? -1 : 1*/0, 1).normalized;
            yield return StartCoroutine(Player.Dash.DashCrt(dashDirection));
        }


        Player.Controller.LockInput = false;
        playerEntryInProgress = false;
    }

    private IEnumerator SwitchSceneCrt(Transition transition)
    {
        playerEntryInProgress = true;
        Player.Controller.LockVelocity = true;

        if(transition.direction == TransitionDirection.Up)
        {
            Player.Controller.gravity = 0;
        }
        SaveLoadManager.Instance.Data.LastExitDirection = (int)transition.direction;
        SaveLoadManager.Instance.Save();

        yield return fader.FadeOut();
        SceneManager.LoadScene(transition.scene, LoadSceneMode.Single);
    }

    private void OnDrawGizmosSelected()
    {
        if(transitions == null)
        {
            return;
        }

        foreach (var t in transitions)
        {
            Gizmos.color = new Color(0, 1, 0, .2f);
            Gizmos.DrawCube(t.bounds.center, t.bounds.size);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(t.bounds.center + t.spawnOffset, .2f);
        }

    }
}
