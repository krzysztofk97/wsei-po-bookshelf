# Bookshelf (wsei-po-bookshelf)

*Projekt semestralny z przedmiotu "Programowanie obiektowe" - prosta aplikacja w języku C# umożliwiająca dostęp i zarządzanie prostą bazą danych*

## Opis projektu

Prosta aplikacja z interfejsem graficznym wykonanym w technologii WPF, służąca do zarządzania biblioteką książek. Dane aplikacji przechowywane są w bazie danych Microsoft SQL Server.

### Funkcje

* Dodawanie książek
* Dodawanie półek i przypisywanie do nich książek
* Dodawanie i przypisywanie autorów do książek
* Dodawanie gatunków i przypisywanie do nich książek
* Modyfikacja zapisanych w bazie danych obiektów

## Instalacja aplikacji

*Aplikacja ze względu na wykorzystanie technologii WPF do tworzenia interfejsu użytkownika, kompatybilna jest jedynie z systemami Windows.*

Aby uruchomić aplikację, należy ją uprzednio skompilować z wykorzystaniem środowiska Visual Studio lub konsolowych narzędzi dotnet w systemie Windows.

Aplikacja do działania wymaga również lokalnej instancji serwera Microsoft SQL Server. Domyślnie w pliku konfiguracyjnym wykorzystywana jest instancja LocalDB. Po uruchomieniu, aplikacja sprawdza, czy instancja SQL Server zawiera w sobie bazę danych programu. Jeśli baza danych nie zostanie odnaleziona, aplikacja zaproponuje stworzenie jej na nowo.

## Użyte technologie

* .NET / C#
* Linq
* WPF
* Entity Framework Core 5
* T-SQL