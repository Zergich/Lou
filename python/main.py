from fuzzywuzzy import fuzz
import stt
import datetime
import webbrowser
import random
from num2words import num2words
import config
import threading
import time
import server
import Voice

#thread = threading.Thread(target=server.Resive)
#thread.start()

print(f"{config.VA_NAME} (v{config.VA_VER}) начал свою работу ...")
recive = threading.Thread(target=server.Resive_msg)
recive.start()

IsActive = False
def ActimeChoose():
    global IsActive
    IsActive = False


def va_respond(voice: str):
    global IsActive

    if voice != "":
        print(voice) 
    if voice.startswith(config.VA_ALIAS) or IsActive == True:
        IsActive = True
        timer = threading.Timer(15.0, ActimeChoose)
        timer.start()
        # обращаются к ассистенту
        cmd = recognize_cmd(filter_cmd(voice), voice)

        #if cmd['cmd'] not in config.VA_CMD_LIST.keys():
             # если команды не найдены то отправляет на ядро и задает вопрос нейрорнке
             #print("что")
        #else:
        if cmd['cmd'] in config.VA_CMD_LIST.keys():
            execute_cmd(cmd['cmd'])


def filter_cmd(raw_voice: str):
    cmd = raw_voice

    for x in config.VA_ALIAS:
        cmd = cmd.replace(x, "").strip()

    for x in config.VA_TBR:
        cmd = cmd.replace(x, "").strip()
    return cmd


def recognize_cmd(cmd: str, voice):
    rc = {'cmd': '', 'percent': 0}
    for c, v in config.VA_CMD_LIST.items():

        for x in v:
            vrt = fuzz.ratio(cmd, x)
            if vrt > rc['percent']:
                rc['cmd'] = c
                rc['percent'] = vrt
    if rc['percent'] >10 and rc['percent'] < 55 and voice.startswith(config.VA_ALIAS):
        print(f"Распознование: {rc['percent']}")
        server.send_message(voice)
    return rc


def execute_cmd(cmd: str):
    if cmd == 'help':
        # help

        #tts.va_speak(text)
        pass
    elif cmd == 'ctime':
        # current time
        now = datetime.datetime.now()
        text = "Сейч+ас " + num2words(now.hour, lang='ru') + " " + num2words (now.minute, lang='ru')
        print(text)
    elif cmd == "shatdownLM":
        server.send_message("<!@CLoseSystemDADAExit1>")
        exit(1)
    elif cmd == "Hide":
        server.send_message("<!UNIIDHIDEC#RESKODLikeUVB76>")
    elif cmd == "Show":
        server.send_message("<!WermaPOajaxzaca21!>")

    else: print("sadasdasdasds")


# начать прослушивание команд
stt.va_listen(va_respond)
