# Table of Content

- [Table of Content](#table-of-content)
- [Introduction](#introduction)
- [Resources](#resources)
  - [General](#general)
  - [Frontend](#frontend)
  - [Backend](#backend)
- [Project Setup](#project-setup)
  - [Frontend](#frontend-1)
  - [Backend](#backend-1)

# Introduction


<!-- Add group introdcution
---------------------- -->

<!-- Michelle's task -->
- Group Name : GalleriaHub
- Group Number : 6
- Group Members : TG Chipoyera 220150124
                  MS Makola 221018369
                AM Ramaili 221000696
                T Damoyi 220002492

<!-- Morena's task -->
- GalleriaHub
- Our Paint Gallery E-Commerce platform is a hub that connects art enthusiasts from different parts of the world. It serves as an online marketplace where painters can showcase and sell their original artworks directly to customers. The platform will cater to both emerging artists and established painters, offering a diverse collection of paintings that cater to various tastes and preferences. By fostering a creative and supportive community, we aim to make art accessible to a broader audience while providing artists with an avenue to reach a global market.



# Resources

These are the resources used to develop GalleriaHub.

## General

- [VSCode](https://code.visualstudio.com/)
- [Git](https://git-scm.com/downloads)
- [GitHub](https://github.com/)
- [Prettier](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode)

## Frontend

- [Nextjs docs](https://nextjs.org/)
- [Tailwind Css](https://tailwindcss.com/)
- [Tailwind CSS IntelliSense](https://marketplace.visualstudio.com/items?itemName=bradlc.vscode-tailwindcss)
- [Nodejs](https://nodejs.org/en)
- [React Icons](https://react-icons.github.io/react-icons/)

## Backend

- [dotnet CLI](https://dotnet.microsoft.com/en-us/download)
- [.Net web api tutorial](https://learn.microsoft.com/en-us/training/modules/build-web-api-minimal-api/1-introduction)
- [Postman](https://www.postman.com/)

# Project Setup

To build GalleriaHub, we needed 2 main things, a front end application (that operates in the browser) & a backend web api that communicates with the database.

To accomplish this, we chose a specific tech-stack.

## Frontend

For the front end, we decided that we going to start with a web application. We wanted to use a modern tool that would make development easier. We chose [NextJs](https://nextjs.org/) as our framwework. There are a few other libraries in use too, but all details pertaining to the web application can be found [here](./client/README.md).

## Backend

For the backend, we decided to use the language that is being taught in school. We'll be using C# and the obvious framework is the [.Net](https://dotnet.microsoft.com/) framework. This is a pure REST web api, not an MVC, webforms or anything else. The reason why we wanted a REST API is because:

- No matter how we decide to make the front end, it will be able to consume the API. Whether we decided to use a javascript framework or whether we raw dogged it with HTML, CSS & Javascript, we had the freedom of choice to build with whatever we were comfortable with.
- There's bonus marks for making a mobile application. Should we have enough time to make the mobile application, the mobile application will be able to consume the same API the web application consumes.

You can read [this](./server/README.md) if you want more information on how the backend was made.
