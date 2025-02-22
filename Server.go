package main

import (
	"fmt"
	"net"
)

func handleConnection(conn net.Conn) {
	// fmt.Println("Новое соединение:", conn.RemoteAddr())

	// Читаем данные из подключения
	buf := make([]byte, 1024)
	n, err := conn.Read(buf)
	if err != nil {
		fmt.Println(err)
		return
	}

	// Обработка данных
	text := string(buf[:n])
	fmt.Println(" <!1UserREsposeMEssage))>", text)
	if text == "<!@CLoseSystemDADAExit1>" {
		fmt.Println("<!@CLoseSystemDADAExit1>")
	} else if text == "<!UNIIDHIDEC#RESKODLikeUVB76>" {
		fmt.Println("<!UNIIDHIDEC#RESKODLikeUVB76>")
	} else if text == "<!WermaPOajaxzaca21!>" {
		fmt.Println("<!WermaPOajaxzaca21!>")
	} else {
		Quest(text)
	}

	// // Отправляем ответ клиенту
	// _, err = conn.Write([]byte("Hello from server!"))
	// if err != nil {
	// 	fmt.Println(err)
	// 	return
	// }
	//
	// fmt.Println("Ответ отправлен")

	// Закрываем соединение
	conn.Close()
}

func SendPy(text string) {
	//		Отправляем ответ клиенту
	conn, err := net.Dial("tcp", "localhost:8081")
	if err != nil {
		fmt.Println("Error connecting:", err.Error())

	}

	conn.Write([]byte(text))
	// Закрываем соединение
	conn.Close()
}

func ServerUp() {
	addr, err := net.ResolveTCPAddr("tcp", "localhost:8080")
	if err != nil {
		panic(err)
	}

	listener, err := net.ListenTCP("tcp", addr)
	if err != nil {
		panic(err)
	}

	// fmt.Println("Сервер запущен")

	for {
		conn, err := listener.Accept()
		if err != nil {
			panic(err)
		}
		go handleConnection(conn)
	}
}
