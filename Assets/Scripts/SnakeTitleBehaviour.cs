using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTitleBehaviour : MonoBehaviour
{
    private Vector2 _direction = Vector2.zero;
    private List<Transform> _segments = new List<Transform>();
    public bool IsDead = false; // bool true if snake shouldve died. resetsnake checks if its invincible or not
    public bool isInvincible = false;
    public Animation rainbowAnimation;
    public Animation SnakeSegRainbowAnim;
    public float coinInvincibleTime = 4f;
    private float IncinvibilityStartTime;
    // private float directionTime;


    public List<Transform> Segments{
        get { return _segments; }
        set { _segments = value; }
    }
    
    public Transform segmentPrefab;
    public int InitialSize = 4;
    // Start is called before the first frame update
    void Start()
    {
        rainbowAnimation = GetComponent<Animation>();
        ResetSnake();
        _direction = Vector2.right;
        // directionTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        // // generate random direction every 2 seconds and change
        // if(Time.time >= directionTime + 0.3f)
        // {
        //     directionTime = Time.time;
        //     // matrix multiply directino by 90 degree clockwise rotation matrix [{0, -1}, {1, 0}]
        //     _direction = new Vector2(_direction.y, -1*_direction.x);
        // }
    }

    void FixedUpdate(){
        // move snake segments behind them
        for(int i = _segments.Count -1; i > 0; i--)
        {
            _segments[i].transform.position = _segments[i-1].transform.position;
        }

        if(this.transform.position.x + _direction.x == 6)
        {
            _direction = new Vector2(_direction.y, -1*_direction.x);
        }
        else if(this.transform.position.y + _direction.y <= -6)
        {
            _direction = new Vector2(_direction.y, -1*_direction.x);
        }
        else if(this.transform.position.x + _direction.x <= -5){
            _direction = new Vector2(_direction.y, -1*_direction.x);
        }
        else if(this.transform.position.y + _direction.y >= 1){
            _direction = new Vector2(_direction.y, -1*_direction.x);
        }
        
        // snake movement
        this.transform.position = new Vector3(
        Mathf.Round(this.transform.position.x) + _direction.x, 
        Mathf.Round(this.transform.position.y) + _direction.y, 
        0.0f);
            

        
        
        
        
        // counter to go in immortal coroutine
        if(isInvincible && (Time.time >= (IncinvibilityStartTime + coinInvincibleTime)))
        {
            turnMortal();
        }

        
    }
    
    private void SpawnSnakeSegments()
    {
        _segments.Add(this.transform); // add snakes head tranform as ref for segments as elment 0 in _segments list
        for (int i = 1; i < InitialSize; i++)
        {
            _segments.Add(Instantiate(segmentPrefab)); // add segment prefab to _segments list
            _segments[i].transform.position = new Vector3(0,0,1); // edit to be 0,0,1 so under snake head
        }
    }

    public void ResetSnake()
    {
        this.transform.position = Vector3.zero; // reset snake pos
        IsDead = false;
        for(int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        SpawnSnakeSegments();
        _direction = Vector2.right;
        //directionTime = Time.time;
    }
    // increase snake size by 1
    private void Grow() 
    {
        if(!isInvincible)
        {
            segmentPrefab.position = _segments[_segments.Count-1].position; // put position = same as prev segment in array
            Transform segment = Instantiate(this.segmentPrefab);
            _segments.Add(segment);
        }
    }

    public void Shrink()
    {
        if(_segments.Count > 1) // make sure theres segments left to remove
        {
            Destroy(_segments[_segments.Count - 1].gameObject);
            _segments.RemoveAt(_segments.Count - 1);
            Debug.Log("segments shrunk, count now = " + _segments.Count);
        }
        else
        {
            if(!isInvincible)
            {
                IsDead = true; 
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Food")
        {
            Grow();
        } 
        else if (other.tag == "Wall")
        {
            // go up or down or left or right.
            if(_direction ==  Vector2.up)
            {
                _direction = Vector2.right;
            }
            else if(_direction == Vector2.left)
            {
                _direction = Vector2.up;
            }
            else if(_direction == Vector2.down){
                _direction = Vector2.left;
            }
            else if(_direction == Vector2.right){
                _direction = Vector2.down;
            }
            if(!isInvincible){
                IsDead = true; 
                }
        }
        else if (other.tag == "Obstacle")
        {
            Debug.Log("collided with segment or wall");
            // snake loose all segments so game manager resets snake and timer. -> because timer inaccesible
            // ResetSnake(); is called from game manger and resets time
            if(!isInvincible)
            {
                IsDead = true;
            }
        }
        else if(other.tag == "Coin")
        {
            Debug.Log("coin = invincible for 5 seconds");
            turnImmortal();
        }
    }
    private void turnImmortal()
    {
        rainbowAnimation.Play();
        SnakeSegRainbowAnim.Play();
        for(int i = 0; i < Segments.Count; i++)
        {
            Segments[i].GetComponent<Animation>().Play();
        }

        isInvincible = true;
        // turn on invicibility colour changing anim for 3 seconds then turn off
        IncinvibilityStartTime = Time.time;
    }

    private void turnMortal()
    {
        rainbowAnimation.Stop();
        for(int i = 0; i < Segments.Count; i++)
        {
            Segments[i].GetComponent<Animation>().Stop();
        }

        isInvincible = false;
    }
}
