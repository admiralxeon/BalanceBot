BalanceBot - Unity ML-Agents ProjectMy first reinforcement learning project using Unity ML-Agents: training a capsule agent to navigate to a target using the PPO algorithm.

🎯 Project OverviewThis project demonstrates fundamental reinforcement learning concepts by training an agent to roll toward a target position in a 3D environment. The agent learns through trial and error using reward signals, ultimately achieving an 87% success rate.Tech Stack:

Unity 6 (2022.3+)
ML-Agents Release 20 (Python 3.10)
PyTorch
TensorBoard for visualization


📊 Training Results
Final Performance Metrics
Training Steps:     340,000
Training Time:      ~40 minutes
Final Mean Reward:  0.873
Std Deviation:      0.065
Success Rate:       ~87%
Convergence Point:  100K steps

Training Curve
The agent showed clear learning progression:
StepsMean RewardStatus10K-1.485Exploring randomly40K+0.795Learning task100K+0.866Near-optimal340K+0.873Plateaued (trained)
Key Insight: The model converged at 100K steps and maintained stable performance for the remaining 240K steps, indicating successful learning.
