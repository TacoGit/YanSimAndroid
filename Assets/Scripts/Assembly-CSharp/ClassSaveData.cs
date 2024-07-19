using System;

[Serializable]
public class ClassSaveData
{
	public int biology;

	public int biologyBonus;

	public int biologyGrade;

	public int chemistry;

	public int chemistryBonus;

	public int chemistryGrade;

	public int language;

	public int languageBonus;

	public int languageGrade;

	public int physical;

	public int physicalBonus;

	public int physicalGrade;

	public int psychology;

	public int psychologyBonus;

	public int psychologyGrade;

	public static ClassSaveData ReadFromGlobals()
	{
		ClassSaveData classSaveData = new ClassSaveData();
		classSaveData.biology = ClassGlobals.Biology;
		classSaveData.biologyBonus = ClassGlobals.BiologyBonus;
		classSaveData.biologyGrade = ClassGlobals.BiologyGrade;
		classSaveData.chemistry = ClassGlobals.Chemistry;
		classSaveData.chemistryBonus = ClassGlobals.ChemistryBonus;
		classSaveData.chemistryGrade = ClassGlobals.ChemistryGrade;
		classSaveData.language = ClassGlobals.Language;
		classSaveData.languageBonus = ClassGlobals.LanguageBonus;
		classSaveData.languageGrade = ClassGlobals.LanguageGrade;
		classSaveData.physical = ClassGlobals.Physical;
		classSaveData.physicalBonus = ClassGlobals.PhysicalBonus;
		classSaveData.physicalGrade = ClassGlobals.PhysicalGrade;
		classSaveData.psychology = ClassGlobals.Psychology;
		classSaveData.psychologyBonus = ClassGlobals.PsychologyBonus;
		classSaveData.psychologyGrade = ClassGlobals.PsychologyGrade;
		return classSaveData;
	}

	public static void WriteToGlobals(ClassSaveData data)
	{
		ClassGlobals.Biology = data.biology;
		ClassGlobals.BiologyBonus = data.biologyBonus;
		ClassGlobals.BiologyGrade = data.biologyGrade;
		ClassGlobals.Chemistry = data.chemistry;
		ClassGlobals.ChemistryBonus = data.chemistryBonus;
		ClassGlobals.ChemistryGrade = data.chemistryGrade;
		ClassGlobals.Language = data.language;
		ClassGlobals.LanguageBonus = data.languageBonus;
		ClassGlobals.LanguageGrade = data.languageGrade;
		ClassGlobals.Physical = data.physical;
		ClassGlobals.PhysicalBonus = data.physicalBonus;
		ClassGlobals.PhysicalGrade = data.physicalGrade;
		ClassGlobals.Psychology = data.psychology;
		ClassGlobals.PsychologyBonus = data.psychologyBonus;
		ClassGlobals.PsychologyGrade = data.psychologyGrade;
	}
}
