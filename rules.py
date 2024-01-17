from dataclasses import dataclass, field
from enum import Enum
from typing import Callable, Any, Dict, Set, List, Tuple, Optional
from datetime import datetime

@dataclass
class Variable:

    name: str
    value: Any
    description: str

class Variables:

    def __init__(self, variables: List[Variable]):
        self.dict: Dict[str, Variable] = {}
        for v in variables:
            self.dict[v.name] = v

    def __getitem__(self, item):
        return self.dict[item]

@dataclass
class MetaRule:
    id: int
    index: int
    name: str
    priority: int

@dataclass
class Rule(MetaRule):
    condition: Callable[[Any, Any, Variables], bool]
    action: Callable[[Any, Any, Variables], Any]
    while_condition: Optional[Callable[[Any, Any, Variables], bool]] = None
    else_action: Optional[Callable[[Any, Any, Variables], Any]] = None

@dataclass
class RuleSet(MetaRule):
    rules: List[Rule] = field(default_factory=list)

    def __getitem__(self, item):
        if type(item) == int:
            return [r for r in self.rules if r.index == item][0]
        else:
            return [r for r in self.rules if r.name == item][0]

@dataclass
class RuleLog:
    rule: Rule
    input: Any
    output: Any
    action: str
    datetime: datetime = datetime.now()

class RuleStatus(Enum):

    START = 1
    SUCCESS = 2
    FAILED = 3
    MAX_ITER = 4

class RuleEngine:

    def __init__(self, ruleset: RuleSet, variables: Variables):
        self.ruleset = ruleset
        self.variables = variables
        self.logs: List[RuleLog] = []
        self.status: RuleStatus = RuleStatus.START
        self.input: Any = None
        self.output: Any = None

    def compile(self, input: Any, output: Any) -> RuleStatus:
        status = self.compile_by_ruleset(input, output, self.ruleset)
        self.output = output
        return status

    def compile_by_ruleset(self, input: Any, output: Any, ruleset: RuleSet) -> Optional[RuleStatus]:
        status = None
        rules = list(ruleset.rules)
        rules.sort(key=lambda r: (r.priority, r.index))
        for rule in rules:
            status = self.predict(input, output, rule)
            if status is not None:
                if status == RuleStatus.FAILED:
                    self.status = RuleStatus.FAILED
                    print(f"FAILED {rule}")
                    break
                elif status == RuleStatus.SUCCESS:
                    self.status = RuleStatus.SUCCESS
                    print(f"SUCCESS {rule}")
                    break
        return status

    def predict(self, input: Any, output: Any, rule: Rule) -> Any:
        o = None
        res = rule.condition(input, output, self.variables)
        if res:
            o = rule.action(input, output, self.variables)
            print(input, output)
            self.logs.append(RuleLog(rule, input, output, "action"))
        elif rule.else_action:
            o = rule.else_action(input, output, self.variables)
            self.logs.append(RuleLog(rule, input, output, "else_action"))
        elif rule.while_condition:
            while rule.while_condition(input, output, self.variables):
                o = rule.action(input, output, self.variables)
                self.logs.append(RuleLog(rule, input, output, "while_condition_action"))
        return o

@dataclass
class Activity:

    id: int
    index: int
    name: str
    engine: RuleEngine

    def next(self, input, output):
        pass

    @property
    def status(self):
        return self.engine.status

class Workflow:

    def __init__(self, id: int, name: str, input: Any, output: Any, activities: List[Activity]):
        self.input = input
        self.output = output
        self.activities = activities


    def start(self):
        self.activities[0].next(self.input, self.output)

    @property
    def logs(self) -> Dict[str, List[RuleLog]]:
        dict = {}
        for a in self.activities:
            dict[a.name] = a.engine.logs
        return dict














