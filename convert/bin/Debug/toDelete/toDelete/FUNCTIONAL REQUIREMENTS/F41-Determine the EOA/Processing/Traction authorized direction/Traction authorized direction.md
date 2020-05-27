﻿
[iTC_CC_ATP-SwRS-0141]
TractionAuthorisedSenseEnd1，如果EOA有效且在END_1方向，则ATP授权列车向END_1方向运行。
If current EOA is valid and whose orientation is END_1, ATP shall authorize the train can move toward END_1.
```
	def TractionAuthorisedSenseEnd1(k):
	    if (EndOfAuthorityValid(k)
	        and TrainFrontEnd(k) is END_1):
	        TractionAuthorisedSenseEnd1 = True
	    else:
	        TractionAuthorisedSenseEnd1 = False
	    return TractionAuthorisedSenseEnd1
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0772]
[End]
[iTC_CC_ATP-SwRS-0142]
TractionAuthorisedSenseEnd2，如果EOA有效且在END_2方向，则ATP授权向驾驶室2方向运行。
If current EOA is valid and whose orientation is END_2, ATP shall authorize the train can move toward END_2.
```
	def TractionAuthorisedSenseEnd2(k):
	    if (EndOfAuthorityValid(k)
	        and TrainFrontEnd(k) is END_2):
	        TractionAuthorisedSenseEnd2 = True
	    else:
	        TractionAuthorisedSenseEnd2 = False
	    return TractionAuthorisedSenseEnd2
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0772]
[End]