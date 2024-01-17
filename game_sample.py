import random
from dataclasses import dataclass
from typing import Optional, List, Any

from rules import Rule, Variables, Variable, RuleStatus, RuleEngine, RuleSet, Activity, Workflow


@dataclass
class GameEntity:
    min: int
    max: int
    value: int

@dataclass
class GameOutput:
    value: int
    nb_try: int = 0

class GameActivity(Activity):

    def __init__(self,id: int, index: int, name: str, engine: RuleEngine,  max_iter = 100):
        super().__init__(id, index, name, engine)
        self.nb_iter = 0
        self.max_iter = max_iter
    def next(self, input, output):
        while self.status == RuleStatus.START:
            self.engine.compile(input, output)
            self.nb_iter += 1
            if self.nb_iter > self.max_iter:
                return RuleStatus.MAX_ITER
        return self.status

class GameWorkflow(Workflow):

    def __init__(self,id: int, name: str, input: GameEntity, output: GameOutput, activities: List[Activity]):
        super().__init__(id, name, input, output, activities)


if __name__ == '__main__':
    input = GameEntity(0, 1000, random.randint(0, 1000))
    output = GameOutput((input.min + input.max) // 2)
    variables = Variables([Variable("version", "0.1", "version")])
    def action_less(i, o, v):
        next = (o.value + i.max) // 2
        if o.value == next:
            next -= 1
        i.min = o.value
        o.value = next
        o.nb_try += 1
        return True

    def action_greater(i, o, v):
        next = (o.value + i.min) // 2
        if o.value == next:
            next += 1
        i.max = o.value
        o.value = next
        o.nb_try += 1
        return True

    rules: List[Rule] = [
        Rule(id=0,
             index=0,
             name="Success",
             priority=10,
             condition = lambda i, o, v: i.value == o.value,
             action = lambda i, o, v: RuleStatus.SUCCESS
            ),
        Rule(id=1,
             index=1,
             name="Less",
             priority=20,
             condition=lambda i, o, v: o.value < i.value,
             action=action_less
             ),
        Rule(id=2,
             index=2,
             name="Greater",
             priority=30,
             condition=lambda i, o, v: o.value > i.value,
             action=action_greater
             ),
        Rule(id=3,
             index=3,
             name="Fail",
             priority=40,
             condition=lambda i, o, v: o.value > i.max or o.value < i.min,
             action=lambda i, o, v: RuleStatus.FAILED
             ),
        Rule(id=4,
             index=4,
             name="Fail2",
             priority=50,
             condition=lambda i, o, v: i.max < i.min,
             action=lambda i, o, v: RuleStatus.FAILED
             ),
    ]
    ruleset = RuleSet(0, 0, "Game", 10, rules)

    engine = RuleEngine(ruleset, variables)
    # engine.compile(input, output)
    # print(engine.logs)
    # print(engine.status)

    activity = GameActivity(0,0,"GameActivity1",engine)
    wf = GameWorkflow(0, "Game", input, output, [activity])
    wf.start()
    print(wf.logs)
    print(wf.activities[0].status)

