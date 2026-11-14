import re

pattern = re.compile(r"(?P<code>\+380) ?(?P<num>[0-9]{9})")
match = pattern.fullmatch("+380 671234567")
if match is not None:
    print(match.group("code"), match.group("num"))
    print(match.span("num"))
print(pattern.fullmatch("+380 671234567\n") is None)
