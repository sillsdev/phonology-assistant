Select Distinct
  dbo.LexEntry.DateCreated,
  dbo.LexEntry.DateModified As Date, 
  dbo.LexSense_Gloss.Txt As Gloss,
  dbo.MoForm_Form.Txt As Phonetic, 
  dbo.CmPossibility_Abbreviation.Txt As POS, 
  dbo.CmObject.Guid$ As Guid,
  dbo.MoForm_Form.Ws
From
  dbo.LexEntry Inner Join 
  dbo.LexEntry_Senses (readuncommitted) On dbo.LexEntry_Senses.Src = dbo.LexEntry.id Inner Join 
  dbo.LexSense (readuncommitted) On dbo.LexEntry_Senses.Dst = dbo.LexSense.id Inner Join 
  dbo.LexSense_Gloss (readuncommitted) On dbo.LexSense_Gloss.Obj = dbo.LexSense.id Inner Join 
  dbo.LexEntry_LexemeForm (readuncommitted) On dbo.LexEntry_LexemeForm.Src = dbo.LexEntry.id Inner Join
  dbo.MoForm (readuncommitted) On dbo.LexEntry_LexemeForm.Dst = dbo.MoForm.id Inner Join 
  dbo.MoForm_Form (readuncommitted) On dbo.MoForm_Form.Obj = dbo.MoForm.id Inner Join 
  dbo.LexEntry_MorphoSyntaxAnalyses (readuncommitted) On dbo.LexEntry_MorphoSyntaxAnalyses.Src = dbo.LexEntry.id Inner Join 
  dbo.MoStemMsa (readuncommitted) On dbo.MoStemMsa.id = dbo.LexEntry_MorphoSyntaxAnalyses.Dst Inner Join 
  dbo.PartOfSpeech (readuncommitted) On dbo.MoStemMsa.PartOfSpeech = dbo.PartOfSpeech.id Inner Join 
  dbo.CmPossibility (readuncommitted) On dbo.PartOfSpeech.id = dbo.CmPossibility.id Inner Join 
  dbo.CmPossibility_Name (readuncommitted) On dbo.CmPossibility_Name.Obj = dbo.CmPossibility.id Inner Join 
  dbo.CmPossibility_Abbreviation (readuncommitted) On dbo.CmPossibility_Abbreviation.Obj = dbo.CmPossibility.id Inner Join 
  dbo.CmObject (readuncommitted) On dbo.LexEntry.id = dbo.CmObject.Id 
Where
  dbo.LexEntry_Senses.Ord = 1 And
  dbo.LexSense_Gloss.Ws = $EnglishGlossWs And 
  dbo.MoForm_Form.Ws = $PhoneticWs And
  dbo.CmPossibility_Name.Ws = $EnglishGlossWs And 
  dbo.CmPossibility_Abbreviation.Ws = $EnglishGlossWs