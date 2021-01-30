using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class EditorInstantiateXObjects : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _objectToInstantiate;
    
    [SerializeField] private Vector2Int _numberObjectToInstantiate;
    
    [Header("Line parameters")]
    [SerializeField]
    private Vector2 _lineSize;
    
    [ContextMenu("Instanciate Objects")]
    public void InstanteObjects()
    {
        if (!AreTheParametersCorrect())
        {
            return;
        }

        Vector3 initialPosition = transform.position - transform.right * _lineSize.x *0.5f;
        Vector3 stepZ = Vector3.forward * _lineSize.y / (_numberObjectToInstantiate.y-1);
        Vector3 stepX = Vector3.right * _lineSize.x / (_numberObjectToInstantiate.x-1);
        {
            for (int i = 0; i < _numberObjectToInstantiate.x; i++)
            {
                Vector3 position = initialPosition + stepZ*i;
                for (int j = 0; j < _numberObjectToInstantiate.y; j++)
                {
                    var gameObject = GameObject.Instantiate(_objectToInstantiate, _parent);
                    gameObject.transform.position = position;

                    //gameObject.transform.localScale = new Vector3 (1,1,rnd);
                    position += stepX;

                }
            }
        }
    }

    private bool AreTheParametersCorrect()
    {
        if (_objectToInstantiate == null)
        {
            Debug.LogError("The Object to instantiate parameter is empty, plase, attach one GameObject");
            return false;
        }

        if (_parent == null)
        {
            Debug.LogError("The Parent where instantiate the new objects, is null");
            return false;
        }

        if (_numberObjectToInstantiate.x < 0 || _numberObjectToInstantiate.y < 0)
        {
            Debug.LogError("The Parent where instantiate the new objects, is null");
            return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
            Debug.DrawRay(transform.position, transform.right * _lineSize.x * 0.5f, Color.magenta);
            Debug.DrawRay(transform.position, -transform.right * _lineSize.x * 0.5f, Color.magenta);
            
            Debug.DrawRay(transform.position, transform.forward * _lineSize.y * 0.5f, Color.magenta);
            Debug.DrawRay(transform.position, -transform.forward * _lineSize.y * 0.5f, Color.magenta);
        
    }
}
