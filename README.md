
# Smart Task Management Tool

Task Management is an intuitive, feature-rich application designed to empower anyone looking to enhance their personal or professional productivity. Task Management provides a seamless experience for organizing, tracking, and prioritizing your dialy tasks.

## Features

- **User Authentication**: Secure sign-up and login processes to keep your tasks private and protected.
- **CRUD Operations**: Full Create, Read, Update, and Delete functionality for tasks, allowing for full control over your task management.
- **Automated Task Descriptions**: Leverage the power of OpenAI to generate detailed descriptions based on task titles, making task entry quick and insightful.
- **Task Management**: Prioritize, and set deadlines for individual tasks, ensuring that you stay on top of your most important activities.

## Technologies Used
- ASP.NET Core: A modern, cross-platform framework for building web applications.
- Authentication: Secure handling of user credentials and sessions.
- Entity Framework Core: An object-database mapper for .NET, ensuring smooth data management.
- OpenAI API: Integration with OpenAI to generate task descriptions and provide AI-assisted insights.
## Run Locally
To run the Task Management application on your local machine, follow these instructions:

### Clone the repository
```bash
git clone https://github.com/mawada-sweis/Task-Management.git
```

### Navigate to the project directory

```bash
  cd Task-Management
```

### Restore dependencies and build the project
```bash
dotnet restore
dotnet build
```

### Run the application
```bash
dotnet run
```
## API Reference

#### Get all items
Retrieves a list of all tasks for the currently authenticated user.
```http
GET /TaskModels
```

#### Get Task Details
Retrieves details of a specific task.
```http
GET /TaskModels/Details/{id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | **Required**.  Id of the task to fetch details for |

#### Create a Task
Creates a new task with the given details and an auto-generated description.
```http
POST /TaskModels/Create
```

Body Parameters:
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `Title`   | `string` | **Required**.  Title of the task |
| `DueDate` | `DateTime` | Due date of the task |
| `Status` | `string` | Status of the task |

#### Edit a Task
Updates the specified task with new details.
```http
POST /TaskModels/Edit/{id}
```
Body Parameters:
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
|`Id`       | `int` | Id of the task (should match URL id)|
| `Title`   | `string` | **Required**.  Title of the task |
|`Description`|`string`|Description of the task|
| `DueDate` | `DateTime` | Due date of the task |
| `Status` | `string` | Status of the task |

#### Delete a Task
Deletes a specific task.
```http
DELETE /TaskModels/Delete/{id}
```
Body Parameters:
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
|`Id`       | `int` | Id of the task to delete)|
