# Папка с проектом
$buildDir = "C:\UnityGames\ShakeNEW\shake"

# Локальный клон вашего Pages-репозитория
$repoDir  = "C:\UnityGames\GitRepos\shake"

# Переходим в репо
Set-Location $repoDir

# 1) Подтягиваем последние изменения
git pull origin main

# 2) Удаляем всё, кроме .git
Get-ChildItem -Force | Where-Object { $_.Name -ne ".git" } | Remove-Item -Recurse -Force

# 3) Копируем свежий билд
Copy-Item "$buildDir\*" -Destination $repoDir -Recurse

# 4) Делаем коммит и пушим
git add .
$ts = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
git commit -m "Auto-deploy WebGL build at $ts"
git push origin main
