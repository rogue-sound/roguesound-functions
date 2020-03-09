# Rogue-Sound Functions project

This projects contains the backend logic used by the main web project (rogue-sound-web). Please refer to the web repository for the main feature list and project explanation.



## Core Team
 
<table>
  <tr>
    <td align="center"><a href="https://github.com/jmolla31"><img src="https://avatars3.githubusercontent.com/u/33100083?s=460&v=4" width="60" alt="jmolla31"/><br /><sub><b>jmolla31</b></sub></a></td>
    <td align="center"><a href="https://github.com/pabravil"><img src="https://avatars2.githubusercontent.com/u/9166688?s=460&v=4" width="60" alt="pabravil"/><br /><sub><b>pabravil</b></sub></a></td>
    <td align="center"><a href="https://github.com/bonavida"><img src="https://avatars2.githubusercontent.com/u/8061481?s=460&v=4" width="60" alt="bonavida"/><br /><sub><b>bonavida</b></sub></a></td>
    <td align="center"><a href="https://github.com/cesarandex"><img src="https://avatars2.githubusercontent.com/u/1353358?s=460&v=4" width="60" alt="cesarandex"/><br /><sub><b>cesarandex</b></sub></a></td>
    <td align="center"><a href="https://github.com/MateoBeMo"><img src="https://avatars1.githubusercontent.com/u/15815193?s=460&v=4" width="60" alt="MateoBeMo"/><br /><sub><b>MateoBeMo</b></sub></a></td>
    <td align="center"><a href="https://github.com/joanstellar"><img src="https://avatars1.githubusercontent.com/u/33447647?s=400&v=4" width="60" alt="joanstellar"/><br /><sub><b>joanstellar</b></sub></a></td>
  </tr>
</table>

## Contributing

We're so glad you're thinking about contributing to Rogue Sound! If you're unsure about anything, just ask —or submit the issue or pull request anyway—. We love all friendly contributions.

If you want to contribute to this project, we encourage you to read the [frontend development guidelines](https://github.com/rogue-sound/development-guidelines/blob/master/FRONTEND_DEVELOPMENT_GUIDELINES.md).

If you have any questions, just [shoot us an email](mailto:rogue.sound.team@gmail.com) :email:.

---
### Main features
- .NetCore 3.1 Azure Functions host.
- CosmosDB as the main datastore.
- User management & authentication with Azure B2C


## Project structure

- RogueSound.Functions => (promptly to be renamed to RogueSound.Player) Contains all the logic related to live rooms (add a new song, get current, skip a song, etc.). 
- RogueSound.Lobby => Contains all the non-live (live music playing) logic related to Rooms & Sessions management (CRUD) and user profiles.
- RogueSound.Common => Common libraries.

## Development environment set-up

Clone the repo and restore the solution using Visual Studio 2019 (not tested with 2017).

You will need a valid CosmosDB connection endpoint and key to start testing the backend, deploy one yourself on Azure or just use the CosmosDB Emulator locally
