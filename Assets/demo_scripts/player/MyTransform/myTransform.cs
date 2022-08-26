using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myTransform : MonoBehaviour
{   
    private Matrix4x4 _transform;

    //explicitly initialize scale to (1,1,1) by default
    protected Vector3 _scale = new Vector3(1,1,1);
    protected Vector3 _position;
    protected Quaternion _rotation;

    public void computeTransform()
    {
        _transform =  Matrix4x4.Translate(_position) * Matrix4x4.Rotate(_rotation) * Matrix4x4.Scale(_scale);

        //update gameobject's position on the current transform matrix
        transform.position = _transform.GetColumn(3);

        //update the game object's rotation based on the current transformation matrix
        transform.rotation = _transform.rotation;

        //update the gameobject's scale
        transform.localScale = _transform.lossyScale;
    }

    //setters-------------------------------------
    public void setPosition(Vector3 pos)
    {
        _position = pos;
    }
    public void setRotation(Quaternion rot)
    {
        _rotation = rot;
    }

    public void setScaleUniform(float s)
    {
        _scale = new Vector3(s, s, s);
    }

    //getters-------------------------------------
    public Vector3 getScale()
    {
        return _scale;
    }
    public Quaternion getRotation()
    {
        return _transform.rotation;
    }
    public Vector3 getPosition()
    {
        return _transform.GetColumn(3);
    }

    public Vector3 getForward()
    {
        return _transform.GetColumn(2).normalized;
    }

    public Vector3 getRight()
    {
        return _transform.GetColumn(0).normalized;
    }


}
