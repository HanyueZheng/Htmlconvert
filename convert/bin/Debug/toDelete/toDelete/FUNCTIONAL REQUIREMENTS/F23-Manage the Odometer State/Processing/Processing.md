
ATP软件每周期在主任务中，需根据对传感器测试结果以及里程计齿数齿号的匹配情况，判断编码里程计的状态OdometerState，并以此计算车轮最大最小位移WheelMaximumMovement和WheelMinimumMovement。
里程计状态分为如下几种，如Figure 57所示：
NOT_INITIALIZED，ATP刚上电后，里程计还未初始化的状态。此时若非已进行传感器测试并检测到车轮完全静止，否则应当过估车轮位移确保安全。
WAITING_COG_POSITION_CODE_READY，ATP刚上电后，从里程计开始转动到连续转过8个齿进行初始化的过程。此过程中应当过估车轮位移，确保安全。
INITIALIZED，里程计经过初始化后正常工作的状态。在此状态下，如果检测到里程计齿数齿号相匹配或者经过传感器测试并检测到车轮完全静止，则使用里程计的读值计算车轮位移；否则，应当过估车轮位移确保安全。
INVALID，里程计的无效状态。
In the main task of each cycle, ATP need to estimate the odometer state and calculate WheelMaximumMovement和WheelMinimumMovement, based on the sensor test results and the matching status between cog count and cog number of odometer. As shown in Figure 57 there are several odometer state: 
NOT_INITIALIZED, just after ATP powered up, the odometer is not initialized. At this moment, ATP should overvalue the wheel displacement to ensure the safety only when the sensor detected that the wheel is completely static.
WAITING_COG_POSITION_CODE_READY, the odometer starts rolling and continues to roll eight cogs. During this process, ATP should over-estimate wheel displacement to ensure the safety. 
INITIALIZED, when odometer has initialized, it enters into the normal working status. If ATP detected the cog count and cog number is matching or the wheel is static, it can calculate the wheel displacement by using the odometer value, otherwise, it should over- estimate the displacement.
INVALID, the odometer is in the invalid status.
Figure 57 Odometer state
