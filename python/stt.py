import json
import pyaudio
import sys
#from vosk import Model, KaldiRecognizer
import vosk
import sounddevice as sd  # pip install sounddevice
import queue

vosk.GpuInit()
#ВСЕ ПРИНТЫ ЗАМЕНИТЬ НА ОТПРАВКУ НА СЕРВЕР
model = vosk.Model(model_name='vosk-model-small-ru-0.22')
samplerate = 16000

q = queue.Queue()

def q_callback(indata, frames, time, status):
    if status:
        print(status, file=sys.stderr)
    q.put(bytes(indata))

def va_listen(callback):
    with sd.RawInputStream(samplerate=samplerate, blocksize=8000, device=sd.default.device, dtype='int16', channels=1, callback=q_callback):
        rec = vosk.KaldiRecognizer(model, samplerate)
        while True:
            data = q.get()
            if rec.AcceptWaveform(data):
                callback(json.loads(rec.Result())["text"])
