package main

// https://github.com/kochetovdv/proxyapi-bot/blob/main/main.go
import (
	"bufio"
	"bytes"
	// "encoding/json"
	"fmt"
	"io"
	"net/http"
	"os"
	"regexp"
	"strings"
)

func main() {
	// Create form data with multiple fields
	InitLuaConfig()
	ServerUp()
}

func Quest(quest string) {
	var builder strings.Builder

	var QuestString = `{ "role": "user", "content": ` + "\"" + quest + "\"" + "}"

	file, err := os.OpenFile("history.cfg", os.O_APPEND|os.O_RDWR, 0600)
	if err != nil {
		fmt.Println("Ощибка открытия файла истории чата", err)
	}
	var history string
	// Читаем файл с начала
	data := make([]byte, 64)

	for {
		n, err := file.Read(data)
		if err == io.EOF { // если конец файла
			break
		}
		history += string((data[:n]))
	}

	defer file.Close()

	builder.WriteString("{\n")
	builder.WriteString(cfg.system + "\n")
	builder.WriteString(history)
	builder.WriteString(QuestString)
	builder.WriteString("\n],")
	builder.WriteString("\n" + cfg.header)
	builder.WriteString("\n}")
	result := builder.String()

	// fmt.Print(result)
	Ansver, errAnsver := GetSend(result)
	if errAnsver != nil {
		panic(errAnsver)
	}
	//тут кароче надо отправить ответ что заокнчило читать
	fmt.Print("<1!Endt-Call>EndBott")

	// errFile := os.WriteFile("1.json", []byte(result), 0644)
	// if errFile != nil {
	// 	fmt.Println("Ошибка записи файла:", errFile)
	// }

	var AnsverSaveString = `{ "role": "assistant", "content": ` + "\"" + strings.Replace(Ansver, "\\", "\\\\", -1) + "\"" + "}"
	file.WriteString(QuestString + ",\n")
	file.WriteString(AnsverSaveString + ",\n")

}

func GetSend(message string) (string, error) {
	// message = `{
	//     "messages": [
	//       { "role": "system", "content": "" },
	//       { "role": "user", "content": "про что я тебя только что спросил?" }
	//     ],
	//   	    "model": "meta-llama-3.1-8b-instruct",
	//     "temperature": 0.6,
	//     "max_tokens": 300,
	//     "stream": true
	// }`
	// скорее всего стрим нужен кок раз таки для эфекта печатной машинки
	var url = "http://26.70.168.18:1234/v1/chat/completions"
	// Send POST request
	client := &http.Client{}

	// buildMessage := cfg.message
	//   buildMessage +=

	data := []byte(message)
	r := bytes.NewReader(data)
	req, err := http.NewRequest("POST", url, r)

	req.Header.Set("Content-Type", "application/json")

	resp, err := client.Do(req)
	if err != nil {
		return "", fmt.Errorf("Ошибка отпраки HTTP запроса", err)
	}
	defer resp.Body.Close()

	fullstring, errAnsver := listenToSSEStream(resp)
	if errAnsver != nil {
		panic(errAnsver)
	}

	return fullstring, nil
}

func listenToSSEStream(resp *http.Response) (string, error) {
	defer resp.Body.Close()
	var FullString string
	reader := bufio.NewReader(resp.Body)
	for {
		line, err := reader.ReadString('\n')
		if err != nil {
			if err == io.EOF {
				break
			}

			return "", fmt.Errorf("Ошибка чтения события: %v", err)
		}

		re := regexp.MustCompile(`"content":"(.*?)"`)

		match := re.FindStringSubmatch(line)
		if match != nil {
			if strings.Contains(match[1], "\\n") {
				fmt.Println()
				continue
			}
			// fmt.Println(match[1])
			fmt.Print(match[1])
			FullString += match[1]
		}

	}
	return FullString, nil
}
