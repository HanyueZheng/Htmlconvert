
对于与列车车身范围有交集的限制区域，例如PSR、TSR、限制状态的PSD或PZ等，ATP需保证列车的瞬时速度不超过该限制区域的限制值。此外，ATP还需保证列车车身范围不能与EOA、非受控道岔、轨道末端等有交集，否则将输出EB。
For the vital zone intersected with the train location, such as PSR, TSR, restricted PSD or Protection Zone etc., ATP shall inhibit the instantaneous velocity of the train will not greater than the limit. In addition, ATP shall request EB if the train location intersected with the EOA, uncontrolled point or track end.
ATP应当保证在最恶劣情况下，能使得列车停在下游的限制点前，即不会冒进EOA。同时，如果下游有降低的PSR或者TSR区域，ATP也应当能够保证列车以低于该限速的速度进入上述区域。为此，ATP需要计算下游各潜在限制点的动能和势能，确保当前列车能量低于限制点能量要求。
Even in the worst cases, ATP should ensure that the train could stop in the upstream of the constraint point, i.e. not exceeding EOA. Meanwhile, if there is PSR or TSR area in the downstream, ATP also should ensure trains enter the area below the speed restriction. Therefore, ATP needs to calculate kinetic energy and static energy of each potential limitation to ensure that the current train energy is lower than the limitation energy.
NOTES：
考虑到软件执行效率，ATP在比较车身范围限制区或下游限制点的能量时，当发现列车能量已经大于某限制区或限制点的能量时，可停止计算下游的奇点能量，直接返回超能结果。就是说，如果列车能量同时超过多个限制区或限制点时，ATP可能只会报出超过最近的一个限制区或限制点的能量。实际上，只要ATP检测出列车能量大于任意一个限制点或限制区，在非MotionProtectionInhibition模式下，都将触发EB，致使列车停止。
