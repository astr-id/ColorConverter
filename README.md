# 🎨 ColorConverter API

**ColorConverter** est une API RESTful ASP.NET Core permettant de convertir des couleurs entre différents formats (HEX, RGB, HSL, CMYK, LAB). L'API est sécurisée par authentification JWT et livrée avec une interface frontend simple pour la tester rapidement.


## Fonctionnalités

-  Conversions de couleur :
    - HEX ↔ RGB
    - RGB → HSL, CMYK, LAB
-  Authentification JWT (compte de test inclus)
-  Architecture propre : Controllers, Services, DTOs
-  Base InMemory pour l’historique des conversions
-  Swagger documenté
-  Interface HTML/CSS/JS

## Démarrage du projet

### 1. Cloner le projet

```bash
git clone https://github.com/tonrepo/colorconverter.git
cd colorconverter
```

### 2. Lancer le backend

```bash
dotnet run
```

## Identifiants de test

- **Nom d’utilisateur** : `jason_admin`
- **Mot de passe** : `MyPass_w0rd`


## Interface POC

### Accès :

> Depuis le navigateur : [https://localhost:5104/index.html](https://localhost:5104/index.html)

### Ce que vous pouvez faire :
- Se connecter via login test
- Taper une couleur (ex : `#ffcc00` ou `rgb(255,255,0)`)
- Sélectionner un format de sortie (HSL, LAB, etc.)
- Cliquer pour **voir la conversion + couleur en direct**
- Copier la couleur convertie avec un clic

## Sécurité (JWT)

- Un token JWT est généré à `/api/login/login`
- Il doit être utilisé dans l'en-tête `Authorization` :
  
  ```
  Authorization: Bearer {token}
  ```

- Les routes protégées sont :
  - `POST /api/color/convert/`
  - `GET /api/color/history`
  - `DELETE /api/color/history/{id}`

## Membre du projet

Projet réalisé par : **Chloé Chiarlini et Astrid Pierron**  
