using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GlowRectFrameCtrl : MonoBehaviour {
    public float radius;
    public float degree;

    public float tarRad;
    public float tarDeg;

    public float spdApproach;

    public LineRenderer line;
    public Vector3[] pos;
    private void Start() {
        line.positionCount = 4;
        line.loop = true;
        RefreshDegree();
    }

    private void RefreshDegree() {
        pos = new Vector3[4];
        for (int i = 0; i < 4; i++) {
            pos[i] = radius * Calc.Deg2Dir3(i * 90f + degree);
        }
        line.SetPositions(pos);
    }

    private void FixedUpdate() {
        if (Input.anyKey) {
            radius.ApproachRef(tarRad, spdApproach);
            degree.ApproachRef(tarDeg, spdApproach);
        }

        RefreshDegree();
    }
}
