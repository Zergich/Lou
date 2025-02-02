package main

import (
	"fmt"

	lua "github.com/yuin/gopher-lua"
)

var cfg LuaConfig

type LuaConfig struct {
	message string
	url     string
	header  string
	system  string
}

func Init() {
	L := lua.NewState()
	if err := L.DoFile("ConfigTest.lua"); err != nil {
		fmt.Print("Загрузка Lua файла", err)
	}

	msg := L.GetGlobal("wakeup").(lua.LString)
	url := L.GetGlobal("url").(lua.LString)

	header := L.GetGlobal("headermessage").(lua.LString)
	system := L.GetGlobal("systempromt").(lua.LString)

	cfg.message = string(msg)
	cfg.url = string(url)
	cfg.system = string(system)
	cfg.header = string(header)
}
