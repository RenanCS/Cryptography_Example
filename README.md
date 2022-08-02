<h1 align="center">
  Apresentação de criptografia aplicada ao backend
</h1>

<h4 align="center">
	🚧  Github Cryptography_Example ♻️ Concluído 🚀 🚧
</h4>

<p align="center">
 <a href="#-sobre-o-projeto">Sobre</a> •
 <a href="#-funcionalidades">Funcionalidades</a> •
 <a href="#-como-executar-o-projeto">Como executar</a> •
 <a href="#-tecnologias">Tecnologias</a> •
 <a href="#-problemas-encontrados">Problemas encontrados</a> •
 <a href="#user-content--licença">Licença</a>
</p>

---

## 💻 Sobre o projeto

O projeto consiste em apresentar dois tipos de criptografia com chave públic e privada, bem como, Aes aplicada ao backend. Foi utilizado no front (.net 6).

Foi criado um middleware para não interferir nas validações dos models.
Dentro do código coloquei alguns comentários necessário para compreender o código.

✅ Arquitetura Limpa <br/>
✅ .Net 6 <br/>

---

## 🚀 Como executar o projeto

Para rodar a criptografia com chave púublica e privado, será necessário gerar os arquivos (**public.perm, private.perm**);

1- Primeiro definir o projeto **CryptographyCreatePem** como inicial;<br>
1.1-Definir o local onde será gerado os arquivos;
1.2-Definir a api **Cryptography** como inicial;
1.3-Dentro do projeto **Cryptography.Library > NewRSA** definir o mesmo caminho onde está os arquivos PERM para realizar a leitura das chaves.
1.4-Dentro do projto **Cryptography > filter > IOMiddleware** definir utilização do **factoryPubPriKey**;

2-Para utilizar a criptografia Aes, será necessário definir a utilização do **factoryAes**

---
## Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:
 [.Net core](https://dotnet.microsoft.com/en-us/download/dotnet/5.0).
Além disto é bom ter um editor para trabalhar com o código como [VSCode](https://code.visualstudio.com/), [Visuall Studio](https://visualstudio.microsoft.com/pt-br/downloads/)


---

## ❌Problemas encontrados

 
---

## 🛠 Tecnologias

- **[.NET](https://dotnet.microsoft.com/en-us/)**
- **[Swagger](https://dotnet.microsoft.comhttps://swagger.io/)**



