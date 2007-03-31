select * from
LanguageProject_AnalysisWritingSystems lp (readuncommitted)
INNER JOIN LgWritingSystem_Name lg (readuncommitted)
ON lp.Dst = lg.Obj