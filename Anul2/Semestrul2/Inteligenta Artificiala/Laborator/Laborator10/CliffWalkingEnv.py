class CliffWalkingEnv:
    def __init__(self,reward_basic,reward_cliff,reward_goal):
        self.rows = 4
        self.columns = 12
        self.start_state = (3,0)
        self.end_state = (3,11)
        self.current_position = None
        self.reward_basic = reward_basic
        self.reward_cliff = reward_cliff
        self.reward_goal = reward_goal

    def transform(self,state):
        return state[0] * self.columns + state[1]

    def reset(self):
        # se returneaza starea initiala -> reprezentarea starii folosind indexarea scalara
        self.current_position = self.start_state
        return self.transform(self.current_position)

    def step(self, action) -> tuple:
        # se considera actiunile urmatoare: 0 UP, 1 DOWN, 2 RIGHT, 3 LEFT
        # returneaza (next_state, reward, done)

        row, col = self.current_position

        if action == 0:
            # goes up
            row = max(0, row - 1)
        elif action == 1:
            # goes down
            row = min(self.rows - 1,row + 1)
        elif action == 2:
            # goes right
            col = min(self.columns - 1,col + 1)
        elif action == 3:
            # goes left
            col = max(0,col - 1)

        # se actualizeaza poztitia curenta
        self.current_position = (row,col)

        # se ofera recompensele
        if self.current_position == self.end_state:
            done = True
            reward = self.reward_goal
        elif row == 3 and col >= 1 and col <= 10:
            # prapastie
            self.reset()
            done = False
            reward = self.reward_cliff
        else:
            # pas normal
            done = False
            reward = self.reward_basic

        return (self.transform(self.current_position), reward, done)

    def render(self):
        # se returneaza pozitia curenta
        return self.transform(self.current_position)