using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[System.Serializable]
public class BoundingBoxCalculator : MonoBehaviour
{
    public bool calculateBoundingBoxes;
    public string platformTag = "platform";
    private GameObject[] _childrenObjects;
    public ObjectBoundaries[] platformBounds;
    private void OnDrawGizmos()
    {
        if ( _childrenObjects !=null && _childrenObjects.Length > 0)
        {
            Gizmos.color = new Color(0.5f, 0.8f, 0.2f, 0.7f);
            if (platformBounds!=null && platformBounds.Length>0)
            {
                foreach (var boundary in platformBounds)
                {
                    Gizmos.DrawCube(boundary.bounds.center,boundary.bounds.size);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (calculateBoundingBoxes)
        {
            calculateBoundingBoxes = false;
            _childrenObjects = new GameObject[this.transform.childCount];
            platformBounds = new ObjectBoundaries[_childrenObjects.Length];
            for (int i = 0; i < _childrenObjects.Length; i++)
            {
                _childrenObjects[i] = this.transform.GetChild(i).gameObject;
                ObjectBoundaries newObj = new ObjectBoundaries();
                newObj.bounds = GetMaxBounds(_childrenObjects[i]);
               
                if (_childrenObjects[i].GetComponent<InGameObject>() !=null)
                {
                    _childrenObjects[i].GetComponent<InGameObject>().boundaries = new ObjectBoundaries();
                    _childrenObjects[i].GetComponent<InGameObject>().boundaries.bounds = newObj.bounds;
                    Vector3 objPosition = _childrenObjects[i].transform.position;
                    newObj.endPosition = new Vector3(objPosition.x,objPosition.y,newObj.bounds.max.z);
                    newObj.positionOffsetVector = new Vector3(0,0,newObj.bounds.extents.z);
                    _childrenObjects[i].GetComponent<InGameObject>().boundaries.endPosition = newObj.endPosition;
                    _childrenObjects[i].GetComponent<InGameObject>().boundaries.positionOffsetVector = newObj.positionOffsetVector;
                    platformBounds[i] = newObj;
                }
               
            }
           
        }
    }
    public Bounds GetMaxBounds(GameObject g,string compareTag = null) {
        var b = new Bounds(g.transform.position, Vector3.zero);
        foreach (Renderer r in g.GetComponentsInChildren<Renderer>()) {
            if (r.gameObject.CompareTag(platformTag))
            {
                b.Encapsulate(r.bounds);
            }
           
        }
        return b;
    }
}
