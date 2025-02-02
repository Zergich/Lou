--  сообзение для спецификации помощника и для запуска модели
wakeup = [[	{
"model": "meta-llama-3.1-8b-instruct",
"temperature": 0.7,
"max_tokens": 150,
"stream": true
"messages": [ { "role": "system", "content": "" } ]}]] -- симтемный промт

url = "http://192.168.0.191:1234/v1/chat/completions"

headermessage = [[
"model": "meta-llama-3.1-8b-instruct",
"temperature": 0.7,
"max_tokens": 200,
"stream": true]]

systempromt = [[
"messages": [
{ "role": "system", "content": "ты индивидуальный помощник. твоя цель отвечать четко и ясно как можно кратко и по теме Хороший ответ - краткий ответ!" },]] -- аккуратно тут нет конца списка сообщений и конца файла(забыл)
