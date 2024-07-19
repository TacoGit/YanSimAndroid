using System.Collections.Generic;
using UnityEngine;

public class YSRetargeting : MonoBehaviour
{
	public GameObject Source;

	public GameObject Target;

	private Component[] SourceSkelNodes;

	private Component[] TargetSkelNodes;

	private Component[] SourceBones;

	private Component[] TargetBones;

	private void Start()
	{
		SourceBones = new Component[160];
		TargetBones = new Component[160];
		SourceSkelNodes = Source.GetComponentsInChildren<Component>();
		TargetSkelNodes = Target.GetComponentsInChildren<Component>();
		string[] array = new string[150]
		{
			"PelvisRoot", "Hips", "LeftUpLeg", "LeftLeg", "LeftFoot", "LeftToes", "LeftToesNull", "LeftLeg_Helper", "LeftHip_Helper", "RightUpLeg",
			"RightLeg", "RightFoot", "RightToes", "RightToesNull", "RightLeg_Helper", "RightHip_Helper", "Front_skart1", "Front_skart2", "Front_skart3", "Left_skart1",
			"Left_skart2", "Left_skart3", "Back_skart1", "Back_skart2", "Back_skart3", "Right_skart1", "Right_skart2", "Right_skart3", "Spine", "Spine1",
			"Spine2", "Spine3", "Neck", "Head", "Head_null", "FrontHair", "BackHair1", "BackHair2", "BackHair3", "BackHair4",
			"LeftHair1", "LeftHair2", "LeftHair3", "RightHair1", "RightHair2", "RightHair3", "face", "atama_n", "mayu", "eyebrow_a_Left",
			"eyebrow_b_Left", "temple_Left", "eyebrow_a_Right", "eyebrow_b_Right", "temple_Right", "eyerid_Left", "eye_Left", "eyelid_und_Left", "cheek_Left", "u_lip_center",
			"lip_top_Left", "lip_top_Right", "jaw", "chin", "b_lip_center", "rip_btm_Left", "lip_u_Left", "rip_btm_Right", "lip_u_Right", "nose",
			"nostrir_Right", "nostrir_Left", "inner_corner_of_eye_Left", "tail_of_eye_Left", "lip_Left", "lip_t_Left", "lip_Right", "lip_t_Right", "eyerid1_Left", "eyerid2_Left",
			"eyelid_und1_Left", "eyelid_und2_Left", "eyerid_Right", "eyerid1_Right", "eyerid2_Right", "tail_of_eye_Right", "eyelid_und2_Right", "eyelid_und_Right", "cheek_Right", "eyelid_und1_Right",
			"eye_Right", "inner_corner_of_eye_Right", "LeftShoulder", "LeftArm", "LeftArmRoll", "LeftForeArm", "LeftForeArmRoll", "LeftHand", "LeftHandThumb1", "LeftHandThumb2",
			"LeftHandThumb3", "LeftHandThumbNull", "LeftHandIndex1", "LeftHandIndex2", "LeftHandIndex3", "LeftHandIndexNull", "LeftHandMiddle1", "LeftHandMiddle2", "LeftHandMiddle3", "LeftHandMiddleNull",
			"LeftHandRing1", "LeftHandRing2", "LeftHandRing3", "LeftHandRingNull", "LeftHandPinky1", "LeftHandPinky2", "LeftHandPinky3", "LeftHandPinkyNull", "LeftForeArm_Helper", "RightShoulder",
			"RightArm", "RightArmRoll", "RightForeArm", "RightForeArmRoll", "RightHand", "RightHandThumb1", "RightHandThumb2", "RightHandThumb3", "RightHandThumbNull", "RightHandIndex1",
			"RightHandIndex2", "RightHandIndex3", "RightHandIndexNull", "RightHandMiddle1", "RightHandMiddle2", "RightHandMiddle3", "RightHandMiddleNull", "RightHandRing1", "RightHandRing2", "RightHandRing3",
			"RightHandRingNull", "RightHandPinky1", "RightHandPinky2", "RightHandPinky3", "RightHandPinkyNull", "RightForeArm_Helper", "LeftBreast", "LeftNipple", "RightBreast", "RightNipple"
		};
		List<string> list = new List<string>();
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		int num = 0;
		for (int j = 0; j < SourceSkelNodes.Length; j++)
		{
			if (list.Contains(SourceSkelNodes[j].name))
			{
				SourceBones[num] = SourceSkelNodes[j];
				num++;
			}
		}
		num = 0;
		for (int k = 0; k < TargetSkelNodes.Length; k++)
		{
			if (list.Contains(TargetSkelNodes[k].name))
			{
				TargetBones[num] = TargetSkelNodes[k];
				num++;
			}
		}
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		for (int i = 0; i < TargetBones.Length; i++)
		{
			if ((bool)TargetBones[i])
			{
				TargetBones[i].transform.localPosition = SourceBones[i].transform.localPosition;
				TargetBones[i].transform.localRotation = SourceBones[i].transform.localRotation;
			}
		}
	}
}
