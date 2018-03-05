using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHazards : MonoBehaviour {

    public GameObject[] hazards;

	void Start () {
        foreach (GameObject hazard in hazards) {
            GameObject newHazard = Instantiate(hazard);
            newHazard.transform.parent = gameObject.transform;
            newHazard.transform.localPosition = new Vector2(Random.Range(-(Helper.UnitScreenWidth() / 2), (Helper.UnitScreenWidth() / 2)), Random.Range(-(Helper.UnitScreenHeight() / 2), (Helper.UnitScreenHeight() / 2)));
        }

	}
	
}
