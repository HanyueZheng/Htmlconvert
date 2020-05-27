
在项目中可以在线路上的部分区域内配置“非强制限制”属性，来要求ATP在满足相应输入条件的情况下认为该区域是限制状态的。“非强制限制”状态的优先级要高于来自该区域变量的实际状态。非强制限制所支持的属性如Table 57所示。
Table 57 Configurable not coerced restrictive identification

|Not Coerced Restrictive|Functional Description|
|-----|
|VARIANTS_RECEIVED_FROM_CBI_ID|用于索引相关联锁的id，取值范围1~32|
|NOT_COERCED_RESTRICTIVE_1|项目配置的非强制限制组合输入1|
|NOT_COERCED_RESTRICTIVE_2|项目配置的非强制限制组合输入2|
|NOT_COERCED_RESTRICTIVE_3|项目配置的非强制限制组合输入3|
|NOT_COERCED_RESTRICTIVE_4|项目配置的非强制限制组合输入4|

[iTC_CC_ATP-SwRS-0677]
NotCoercedRestrictive_1，非强制限制1
```
	def NotCoercedRestrictive_1(k):
	    return Offline.GetNotCoercedRestrictive_1(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1275], [iTC_CC_ATP_SwHA-0256]
[End]
[iTC_CC_ATP-SwRS-0678]
NotCoercedRestrictive_2，非强制限制2
```
	def NotCoercedRestrictive_2(k):
	    return Offline.GetNotCoercedRestrictive_2(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1275], [iTC_CC_ATP_SwHA-0256]
[End]
[iTC_CC_ATP-SwRS-0679]
NotCoercedRestrictive_3，非强制限制3
```
	def NotCoercedRestrictive_3(k):
	    return Offline.GetNotCoercedRestrictive_3(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1275], [iTC_CC_ATP_SwHA-0256]
[End]
[iTC_CC_ATP-SwRS-0680]
NotCoercedRestrictive_4，非强制限制4
```
	def NotCoercedRestrictive_4(k):
	    return Offline.GetNotCoercedRestrictive_4(k)
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-1275], [iTC_CC_ATP_SwHA-0256]
[End]
[iTC_CC_ATP-SwRS-0681]
CoercedRestrictive，等于相应的“非强制限制”取反。
```
	def CoercedRestrictive(ncr, k):
	    if (ncr is NOT_COERCED_RESTRICTIVE_1):
	        CoercedRestrictive = not NotCoercedRestrictive_1(k)
	    elif (ncr is NOT_COERCED_RESTRICTIVE_2):
	        CoercedRestrictive = not NotCoercedRestrictive_2(k)
	    elif (ncr is NOT_COERCED_RESTRICTIVE_3):
	        CoercedRestrictive = not NotCoercedRestrictive_3(k)
	    elif (ncr is NOT_COERCED_RESTRICTIVE_4):
	        CoercedRestrictive = not NotCoercedRestrictive_4(k)
	    elif (ncr is VARIANTS_RECEIVED_FROM_CBI_ID):
	        CoercedRestrictive = not CBIvariantLowValidity(VARIANTS_RECEIVED_FROM_CBI_ID, k)
	    else:
	        CoercedRestrictive = False
	    return CoercedRestrictive
```
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-1275], [iTC_CC_ATP_SwHA-0256]
[End]
