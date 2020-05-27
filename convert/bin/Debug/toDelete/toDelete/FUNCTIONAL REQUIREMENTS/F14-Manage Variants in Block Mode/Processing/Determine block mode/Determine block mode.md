
[iTC_CC_ATP-SwRS-0067]
BlockModeUsed，当前是否现在选择BM模式。其状态来自于项目可配置的列车输入采集。
BlockModeUsed represents that either of train end chooses BM mode. 
```
	def BlockModeUsed(k):
	    return Offline.GetBlockModeUsed(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0948], [iTC_CC-SyAD-1003], [iTC_CC_ATP_SwHA-0203]
[End]
[iTC_CC_ATP-SwRS-0066]
BMvariantValidWhileTemporallyValid，当前是否使用BM变量。其状态来自于项目可配置的列车输入采集。
The status of BMvariantValidWhileTemporallyValid shows whether it is in the BM mode.  
```
	def BMvariantValidWhileTemporallyValid(k):
	    return Offline.GetBMvariantValidWhileTemporallyValid(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0298], [iTC_CC-SyAD-0841], [iTC_CC-SyAD-1003], [iTC_CC_ATP_SwHA-0202]
[End]
