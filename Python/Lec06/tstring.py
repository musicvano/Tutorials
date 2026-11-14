name = "<b>Оля</b>"
template = t"Вітаємо, {name}!"
print(type(template).__name__)
print(template.strings)
field = template.interpolations[0]
print(field.value, field.expression)
print(field.conversion, repr(field.format_spec))
