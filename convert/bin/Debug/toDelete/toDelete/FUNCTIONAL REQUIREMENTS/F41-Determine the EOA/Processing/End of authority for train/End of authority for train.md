
[iTC_CC_ATP-SwRS-0671]
EndOfAuthorityValid，统一BM或CBTC下的EOA是否可用。
```
	def EndOfAuthorityValid(k):
	    if (BlockModeUsed(k)):
	        return BlockModeEOAvalid(k)
	    else:
	        return CBTCmodeEOAvalid(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0289], [iTC_CC-SyAD-0292], [iTC_CC-SyAD-0293], [iTC_CC-SyAD-0913]
[End]
