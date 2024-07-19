using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StudentSaveData
{
	public bool customSuitor;

	public int customSuitorAccessory;

	public int customSuitorBlonde;

	public int customSuitorEyewear;

	public int customSuitorHair;

	public int customSuitorJewelry;

	public bool customSuitorTan;

	public int expelProgress;

	public int femaleUniform;

	public int maleUniform;

	public IntAndStringDictionary studentAccessory = new IntAndStringDictionary();

	public IntHashSet studentArrested = new IntHashSet();

	public IntHashSet studentBroken = new IntHashSet();

	public IntAndFloatDictionary studentBustSize = new IntAndFloatDictionary();

	public IntAndColorDictionary studentColor = new IntAndColorDictionary();

	public IntHashSet studentDead = new IntHashSet();

	public IntHashSet studentDying = new IntHashSet();

	public IntHashSet studentExpelled = new IntHashSet();

	public IntHashSet studentExposed = new IntHashSet();

	public IntAndColorDictionary studentEyeColor = new IntAndColorDictionary();

	public IntHashSet studentGrudge = new IntHashSet();

	public IntAndStringDictionary studentHairstyle = new IntAndStringDictionary();

	public IntHashSet studentKidnapped = new IntHashSet();

	public IntHashSet studentMissing = new IntHashSet();

	public IntAndStringDictionary studentName = new IntAndStringDictionary();

	public IntHashSet studentPhotographed = new IntHashSet();

	public IntHashSet studentReplaced = new IntHashSet();

	public IntAndIntDictionary studentReputation = new IntAndIntDictionary();

	public IntAndFloatDictionary studentSanity = new IntAndFloatDictionary();

	public IntHashSet studentSlave = new IntHashSet();

	public static StudentSaveData ReadFromGlobals()
	{
		StudentSaveData studentSaveData = new StudentSaveData();
		studentSaveData.customSuitor = StudentGlobals.CustomSuitor;
		studentSaveData.customSuitorAccessory = StudentGlobals.CustomSuitorAccessory;
		studentSaveData.customSuitorBlonde = StudentGlobals.CustomSuitorBlonde;
		studentSaveData.customSuitorEyewear = StudentGlobals.CustomSuitorEyewear;
		studentSaveData.customSuitorHair = StudentGlobals.CustomSuitorHair;
		studentSaveData.customSuitorJewelry = StudentGlobals.CustomSuitorJewelry;
		studentSaveData.customSuitorTan = StudentGlobals.CustomSuitorTan;
		studentSaveData.expelProgress = StudentGlobals.ExpelProgress;
		studentSaveData.femaleUniform = StudentGlobals.FemaleUniform;
		studentSaveData.maleUniform = StudentGlobals.MaleUniform;
		int[] array = StudentGlobals.KeysOfStudentAccessory();
		foreach (int num in array)
		{
			studentSaveData.studentAccessory.Add(num, StudentGlobals.GetStudentAccessory(num));
		}
		int[] array2 = StudentGlobals.KeysOfStudentArrested();
		foreach (int num2 in array2)
		{
			if (StudentGlobals.GetStudentArrested(num2))
			{
				studentSaveData.studentArrested.Add(num2);
			}
		}
		int[] array3 = StudentGlobals.KeysOfStudentBroken();
		foreach (int num3 in array3)
		{
			if (StudentGlobals.GetStudentBroken(num3))
			{
				studentSaveData.studentBroken.Add(num3);
			}
		}
		int[] array4 = StudentGlobals.KeysOfStudentBustSize();
		foreach (int num4 in array4)
		{
			studentSaveData.studentBustSize.Add(num4, StudentGlobals.GetStudentBustSize(num4));
		}
		int[] array5 = StudentGlobals.KeysOfStudentColor();
		foreach (int num5 in array5)
		{
			studentSaveData.studentColor.Add(num5, StudentGlobals.GetStudentColor(num5));
		}
		int[] array6 = StudentGlobals.KeysOfStudentDead();
		foreach (int num6 in array6)
		{
			if (StudentGlobals.GetStudentDead(num6))
			{
				studentSaveData.studentDead.Add(num6);
			}
		}
		int[] array7 = StudentGlobals.KeysOfStudentDying();
		foreach (int num8 in array7)
		{
			if (StudentGlobals.GetStudentDying(num8))
			{
				studentSaveData.studentDying.Add(num8);
			}
		}
		int[] array8 = StudentGlobals.KeysOfStudentExpelled();
		foreach (int num10 in array8)
		{
			if (StudentGlobals.GetStudentExpelled(num10))
			{
				studentSaveData.studentExpelled.Add(num10);
			}
		}
		int[] array9 = StudentGlobals.KeysOfStudentExposed();
		foreach (int num12 in array9)
		{
			if (StudentGlobals.GetStudentExposed(num12))
			{
				studentSaveData.studentExposed.Add(num12);
			}
		}
		int[] array10 = StudentGlobals.KeysOfStudentEyeColor();
		foreach (int num14 in array10)
		{
			studentSaveData.studentEyeColor.Add(num14, StudentGlobals.GetStudentEyeColor(num14));
		}
		int[] array11 = StudentGlobals.KeysOfStudentGrudge();
		foreach (int num16 in array11)
		{
			if (StudentGlobals.GetStudentGrudge(num16))
			{
				studentSaveData.studentGrudge.Add(num16);
			}
		}
		int[] array12 = StudentGlobals.KeysOfStudentHairstyle();
		foreach (int num18 in array12)
		{
			studentSaveData.studentHairstyle.Add(num18, StudentGlobals.GetStudentHairstyle(num18));
		}
		int[] array13 = StudentGlobals.KeysOfStudentKidnapped();
		foreach (int num20 in array13)
		{
			if (StudentGlobals.GetStudentKidnapped(num20))
			{
				studentSaveData.studentKidnapped.Add(num20);
			}
		}
		int[] array14 = StudentGlobals.KeysOfStudentMissing();
		foreach (int num22 in array14)
		{
			if (StudentGlobals.GetStudentMissing(num22))
			{
				studentSaveData.studentMissing.Add(num22);
			}
		}
		int[] array15 = StudentGlobals.KeysOfStudentName();
		foreach (int num24 in array15)
		{
			studentSaveData.studentName.Add(num24, StudentGlobals.GetStudentName(num24));
		}
		int[] array16 = StudentGlobals.KeysOfStudentPhotographed();
		foreach (int num26 in array16)
		{
			if (StudentGlobals.GetStudentPhotographed(num26))
			{
				studentSaveData.studentPhotographed.Add(num26);
			}
		}
		int[] array17 = StudentGlobals.KeysOfStudentReplaced();
		foreach (int num28 in array17)
		{
			if (StudentGlobals.GetStudentReplaced(num28))
			{
				studentSaveData.studentReplaced.Add(num28);
			}
		}
		int[] array18 = StudentGlobals.KeysOfStudentReputation();
		foreach (int num30 in array18)
		{
			studentSaveData.studentReputation.Add(num30, StudentGlobals.GetStudentReputation(num30));
		}
		int[] array19 = StudentGlobals.KeysOfStudentSanity();
		foreach (int num32 in array19)
		{
			studentSaveData.studentSanity.Add(num32, StudentGlobals.GetStudentSanity(num32));
		}
		return studentSaveData;
	}

	public static void WriteToGlobals(StudentSaveData data)
	{
		StudentGlobals.CustomSuitor = data.customSuitor;
		StudentGlobals.CustomSuitorAccessory = data.customSuitorAccessory;
		StudentGlobals.CustomSuitorBlonde = data.customSuitorBlonde;
		StudentGlobals.CustomSuitorEyewear = data.customSuitorEyewear;
		StudentGlobals.CustomSuitorHair = data.customSuitorHair;
		StudentGlobals.CustomSuitorJewelry = data.customSuitorJewelry;
		StudentGlobals.CustomSuitorTan = data.customSuitorTan;
		StudentGlobals.ExpelProgress = data.expelProgress;
		StudentGlobals.FemaleUniform = data.femaleUniform;
		StudentGlobals.MaleUniform = data.maleUniform;
		foreach (KeyValuePair<int, string> item in data.studentAccessory)
		{
			StudentGlobals.SetStudentAccessory(item.Key, item.Value);
		}
		foreach (int item2 in data.studentArrested)
		{
			StudentGlobals.SetStudentArrested(item2, true);
		}
		foreach (int item3 in data.studentBroken)
		{
			StudentGlobals.SetStudentBroken(item3, true);
		}
		foreach (KeyValuePair<int, float> item4 in data.studentBustSize)
		{
			StudentGlobals.SetStudentBustSize(item4.Key, item4.Value);
		}
		foreach (KeyValuePair<int, Color> item5 in data.studentColor)
		{
			StudentGlobals.SetStudentColor(item5.Key, item5.Value);
		}
		foreach (int item6 in data.studentDead)
		{
			StudentGlobals.SetStudentDead(item6, true);
		}
		foreach (int item7 in data.studentDying)
		{
			StudentGlobals.SetStudentDying(item7, true);
		}
		foreach (int item8 in data.studentExpelled)
		{
			StudentGlobals.SetStudentExpelled(item8, true);
		}
		foreach (int item9 in data.studentExposed)
		{
			StudentGlobals.SetStudentExposed(item9, true);
		}
		foreach (KeyValuePair<int, Color> item10 in data.studentEyeColor)
		{
			StudentGlobals.SetStudentEyeColor(item10.Key, item10.Value);
		}
		foreach (int item11 in data.studentGrudge)
		{
			StudentGlobals.SetStudentGrudge(item11, true);
		}
		foreach (KeyValuePair<int, string> item12 in data.studentHairstyle)
		{
			StudentGlobals.SetStudentHairstyle(item12.Key, item12.Value);
		}
		foreach (int item13 in data.studentKidnapped)
		{
			StudentGlobals.SetStudentKidnapped(item13, true);
		}
		foreach (int item14 in data.studentMissing)
		{
			StudentGlobals.SetStudentMissing(item14, true);
		}
		foreach (KeyValuePair<int, string> item15 in data.studentName)
		{
			StudentGlobals.SetStudentName(item15.Key, item15.Value);
		}
		foreach (int item16 in data.studentPhotographed)
		{
			StudentGlobals.SetStudentPhotographed(item16, true);
		}
		foreach (int item17 in data.studentReplaced)
		{
			StudentGlobals.SetStudentReplaced(item17, true);
		}
		foreach (KeyValuePair<int, int> item18 in data.studentReputation)
		{
			StudentGlobals.SetStudentReputation(item18.Key, item18.Value);
		}
		foreach (KeyValuePair<int, float> item19 in data.studentSanity)
		{
			StudentGlobals.SetStudentSanity(item19.Key, item19.Value);
		}
	}
}
