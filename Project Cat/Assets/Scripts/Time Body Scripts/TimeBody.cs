using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    [SerializeField] KeyCode rewindKey = KeyCode.LeftControl;
    [SerializeField] float maxRecordTime;
    public float MaxRecordTime { get => maxRecordTime; }

    bool isRewinding = false;
    public bool IsRewinding { get => isRewinding; }

    List<Vector3> positions;

    RecordedAnimationState recordedAnimationState;

    void Awake() {
        recordedAnimationState = GetComponent<RecordedAnimationState>();
    }

    void Start()
    {
        positions = new List<Vector3>();
    }

    void Update()
    {
        if (Input.GetKey(rewindKey)) StartRewind();
        else if (!Input.GetKey(rewindKey)) StopRewind();
    }

    private void FixedUpdate() {
        if (isRewinding) Rewind();
        else Record();
    }

    void Rewind()
    {
        if (positions.Count > 0)
        {
            transform.position = positions[0];
            positions.RemoveAt(0);
        }

        else StopRewind();
    } 

    void Record()
    {
        if (positions.Count > Mathf.Round((1f / Time.fixedDeltaTime) * maxRecordTime))
        {
            positions.RemoveAt(positions.Count - 1); 
        }

        positions.Insert(0, transform.position);    
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}
