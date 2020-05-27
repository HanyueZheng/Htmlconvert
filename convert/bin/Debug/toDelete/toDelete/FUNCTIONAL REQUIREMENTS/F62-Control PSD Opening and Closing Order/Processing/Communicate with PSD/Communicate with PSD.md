
判断列车是否与PSD区域有交集，并根据CCNV的请求与PSD通信。
[iTC_CC_ATP-SwRS-0266]
AlignPSDzone_A，列车定位与A侧PSD区有交集；
PSDid_A，与列车定位有交集的A侧PSD的id号。
ATP初始化或者失位时，默认设置AlignPSDzone_A为False，PSDid_A为None；
当车身定位（即从车尾最小定位到车头最大定位）与屏蔽门区域（线路地图TrackMap中由一对方向相反的SGL_PSD_ZONE奇点组成）有交集，且根据Table 512判断为A侧时，设置AlignPSDzone_A为True，并将PSDid_A为设置为该SGL_PSD_ZONE奇点的id。
其他情况，设置AlignPSDzone_A为False，PSDid_A为None。
AlignPSDzone_A, ATP shall determine whether there are intersection between the range of train locations and the platform screen doors zone of the side-A.
PSDid_A, the id of the PSD on side-A intersects with train location.
In initialization or train delocalization, set AlignPSDzone_A as False and PSDid_A as None.
If there are intersection between the range of train locations (from the minimum train tail to the maximum train head) and the PSD zone (composing by a pair of SGL_PSD_ZONE singularities with opposite direction in the train map), and the train door side A correspond to the platform according with Table 512, set AlignPSDzone_A as True, and records PSDid_A as the PSD’s id:
Otherwise, set AlignPSDzone_A as False and PSDid_A as None.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0261], [iTC_CC_ATP_SwHA-0107], [iTC_CC_ATP_SwHA-0109]
[End]
[iTC_CC_ATP-SwRS-0268]
AlignPSDzone_B，列车定位与B侧PSD区有交集；
PSDid_B，与列车定位有交集的B侧PSD的id号。
ATP初始化或者失位时，默认设置AlignPSDzone_B为False，PSDid_B为None；
当车身定位（即从车尾最小定位到车头最大定位）与屏蔽门区域（线路地图TrackMap中由一对方向相反的SGL_PSD_ZONE奇点组成）有交集，且根据Table 512判断为B侧时，设置AlignPSDzone_B为True，并将PSDid_B为设置为该SGL_PSD_ZONE奇点的id。
其他情况，设置AlignPSDzone_B为False，PSDid_B为None。
AlignPSDzone_B, ATP shall determine whether there are intersection between the range of train locations and the platform screen doors zone of the side-A.
PSDid_B, the id of the PSD on side-A intersects with train location.
In initialization or train delocalization, set AlignPSDzone_B as False and PSDid_B as None.
If there are intersection between the range of train locations (from the minimum train tail to the maximum train head) and the PSD zone (composing by a pair of SGL_PSD_ZONE singularities with opposite direction in the train map), and the train door side B correspond to the platform according with Table 512, set AlignPSDzone_B as True, and records PSDid_B as the PSD’s id:
Otherwise, set AlignPSDzone_B as False and PSDid_B as None.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0261], [iTC_CC_ATP_SwHA-0107], [iTC_CC_ATP_SwHA-0109]
[End]
[iTC_CC_ATP-SwRS-0136]
PSDoperation_A和PSDoperation_B，其结构为ST_PSD_OPERATION，用于获取来自CCNV的屏蔽门控制指令。
PSDoperation_A and PSDoperation_B structured as ST_PSD_OPERATION, used to obtain the PSD controlling order from CCNV.
```
	if (ATOcontrolTimeValid(k) == True)
	    PSDoperation_A(k)= NonVitalRequest.PSDoperation_A(k)
	    PSDoperation_B(k)= NonVitalRequest.PSDoperation_B(k)
	else:
	    PSDoperation_A(k).Id = None
	    PSDoperation_B(k).Id = None
```
\#Category= Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0068], [iTC_CC-SyAD-1044]
[End]
[iTC_CC_ATP-SwRS-0467]
CommunicateWithPSD，ATP根据CCNV的请求，判断是否与联锁建立通信。
当本周期来自CCNV的PSDoperation_A或PSDoperation_B不全为None时，设置CommunicateWithPSD为True；
否则，设置CommunicateWithPSD为False。
ATP shall determine whether to establish communication with the correlative CI according to request from CCNV:
When there is at least one id of PSDoperation_A or PSDoperation_B is not none, ATP shall set CommunicatedWithPSD to True:
Otherwise, set CommunicatedWithPSD to False.
```
	def CommunicateWithPSD(k):
	    return (PSDoperation_A(k).Id is not None
	            or PSDoperation_B(k).Id is not None)
```
\#Category=Functional
\#Contribution=SIL0
\#Allocation=ATP Software
\#Source=[iTC_CC-SyAD-0068], [iTC_CC-SyAD-0265]
[End]
当列车运行在配置有PSD的车站时，ATP需与联锁建立通信，获取PSD的状态信息。根据配置数据，ATP软件维护来自CI的PSD的状态信息数组TableOfPSDPlatform，其索引就是PSD的id值，每个数组元素如ST_PSD_TABLE所示： 

|Identification|Logical Type|Description|
|-----|
|ST_PSD_TABLE|||
||DoorStatusValidityTime| REF NUMERIC_32 \h NUMERIC_32|PSD状态是否有效|
||DoorClosed| REF BOOLEAN \h BOOLEAN|PSD状态|

When the train is running in the station equipped with PSD, ATP needs to communicate with CI and get the PSD status information. According to the configuration data, ATP will process the PSD from CI, and the index is the id of PSD, just as shown in the ST_PSD_TABLE
[iTC_CC_ATP-SwRS-0111]
初始化时，设置TableOfPSDPlatform数组中所有PSD的DoorClosed均为False，其有效期为0；此后，如果本周期收到正确的来自CI的CI_IOstatus消息时，根据其ID号，更新TableOfPSDPlatform数组中相应PSD的DoorClosed状态，并将DoorStatusValidityTime设为ATPsetting.PSDstatusValidityTime减去FSFB2消息传输延迟。
In initialization, all PSD DoorClosed of TableOfPSDPlatform set as False, and the valid period is zero. Later on, if receiving correct CI_IOstatus, on the basis of ID number the status of related PSD doorClosed from TableOfPSDPlatform is updated and the DoorStatusValidityTime is set as ATPsetting.PSDstatusValidityTime subtracted the delay of FSFB2 message.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0949], [iTC_CC_ATP_SwHA-0032], [iTC_CC_ATP_SwHA-0276]
[End]
[iTC_CC_ATP-SwRS-0112]
如果本周期未收到来自CI的CI_IOstatus消息，或者该消息校验错误，则ATP更新TableOfPSDPlatform数组中的PSD的状态。即将DoorStatusValidityTime减1，如果该值已小于等于0，则将DoorClosed 设为False；否则保持DoorClosed不变。
If ATP does not receive the CI_IOstatus from CI, or if this message detected as False, ATP shall update the PSD status of TableOfPSDPlatform, i.e. it is necessary to minus DoorStatusValidityTime with one. If the value is less than or equal to zero, the status of DoorClosed is set as False; otherwise the status keeps the same.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software
\#Source=[iTC_CC_ATP_SwHA-0032], [iTC_CC_ATP_SwHA-0276]
[End]
