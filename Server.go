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
	Quest(text)

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

//	func handleSebderCS(conn net.Conn) {
//		// // Отправляем ответ клиенту
//
//		buf := make([]byte, 1024)
//		_, err := conn.Read(buf)
//		_, err = conn.Write([]byte("Hello from server!"))
//		if err != nil {
//			fmt.Println(err)
//			return
//		}
//
//
//		// Закрываем соединение
//		conn.Close()
//
// }
// func ServerCS() {
//
// 	addr, err := net.ResolveTCPAddr("tcp", "localhost:8085")
// 	if err != nil {
// 		panic(err)
// 	}
//
// 	listener, err := net.ListenTCP("tcp", addr)
// 	if err != nil {
// 		panic(err)
// 	}
//
// 	// fmt.Println("Сервер запущен")
//
// 	for {
// 		conn, err := listener.Accept()
// 		if err != nil {
// 			panic(err)
// 		}
// 	}
//
// }

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
