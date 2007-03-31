select * from
LanguageProject_VernacularWritingSystems lp (readuncommitted)
INNER JOIN LgWritingSystem_Name lg (readuncommitted)
ON lp.Dst = lg.Obj
