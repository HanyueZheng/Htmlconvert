
[iTC_CC_ATP-SwRS-0273]
EvacuationNotPossible_A，禁止A侧逃生。
初始化或TrainLocatedOnKnownPath为False时，默认EvacuationNotPossible_A为False。
否则，当车身定位（即从车尾最小定位到车头最大定位）与禁止逃生区（线路地图TrackMap中由一对方向相反的SGL_HAZAR_EVAC_ZONE奇点组成）范围有交集，且根据Table 512判断为A侧时，设置EvacuationNotPossible_A为True：
其他情况，设置EvacuationNotPossible_A为False。
ATP shall consider the evacuation is not possible on side-A when there are intersection between the range of train locations and the hazardous evacuation zone of the side-A:
In initialization or not TrainLocatedOnKnownPath, set the EvacuationNotPossible_A as False;
Or else, if there are intersection between the range of train locations (from the minimum train tail to the maximum train head) and the vital passenger exchange zone (composing by a pair of SGL_HAZAR_EVAC_ZONE singularities with opposite direction in the train map), and the train door side A correspond to the EVAC according with Table 512, set EvacuationNotPossible_A as True.
Otherwise, set EvacuationNotPossible_A as False.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0244], [iTC_CC_ATP_SwHA-0112]
[End]
[iTC_CC_ATP-SwRS-0274]
EvacuationNotPossible_B，禁止B侧逃生。
初始化或TrainLocatedOnKnownPath为False时，默认EvacuationNotPossible_B为False。
否则，当车身定位（即从车尾最小定位到车头最大定位）与禁止逃生区（线路地图TrackMap中由一对方向相反的SGL_HAZAR_EVAC_ZONE奇点组成）范围有交集，且根据Table 512判断为B侧时，设置EvacuationNotPossible_B为True。
其他情况，设置EvacuationNotPossible_B为False。
ATP shall consider the evacuation is not possible on side-B when there are intersection between the range of train locations and the hazardous evacuation zone of the side-B:
In initialization or not TrainLocatedOnKnownPath, set the EvacuationNotPossible_B as False;
Or else, if there are intersection between the range of train locations (from the minimum train tail to the maximum train head) and the vital passenger exchange zone (composing by a pair of SGL_HAZAR_EVAC_ZONE singularities with opposite direction in the train map), and the train door side B correspond to the EVAC according with Table 512, set EvacuationNotPossible_B as True.
Otherwise, set EvacuationNotPossible_B as False.
\#Category=Functional
\#Contribution=SIL4
\#Allocation=ATP Software, Vital Embedded Setting
\#Source=[iTC_CC-SyAD-0244], [iTC_CC_ATP_SwHA-0112]
[End]
