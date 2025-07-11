# WSL2からWindows CLIツール実行要領書

## 概要

WSL2 (Linux) 環境から Windows上の任意のCLIツールを実行し、結果を取得する方法を解説します。

## 基本原理

### ファイルシステムマウント
- Windows の `C:\` ドライブは WSL2 では `/mnt/c/` としてマウントされる
- Windows の実行ファイル (`.exe`) は WSL2 から直接実行可能

### パス変換ルール
```
Windows側パス          → WSL2側パス
C:\Program Files\     → /mnt/c/Program Files/
C:\Users\             → /mnt/c/Users/
C:\Windows\System32\  → /mnt/c/Windows/System32/
D:\Tools\             → /mnt/d/Tools/
```

## 基本実行構文

### 1. 基本形式
```bash
"/mnt/c/path/to/program.exe" [引数] [オプション]
```

### 2. スペースを含むパスの場合
```bash
# 正しい例
"/mnt/c/Program Files/Tool/tool.exe" --option value

# 間違った例（スペースでエラーになる）
/mnt/c/Program Files/Tool/tool.exe --option value
```

### 3. 相対パスの場合
```bash
# 現在のディレクトリが /mnt/c/Users/username/project の場合
"./tool.exe" --option value
```

## 主要CLIツールの実行例

### .NET CLI
```bash
# バージョン確認
"/mnt/c/Program Files/dotnet/dotnet.exe" --version

# プロジェクトビルド
"/mnt/c/Program Files/dotnet/dotnet.exe" build

# テスト実行
"/mnt/c/Program Files/dotnet/dotnet.exe" test --verbosity normal
```

### Node.js
```bash
# Node.jsバージョン確認
"/mnt/c/Program Files/nodejs/node.exe" --version

# NPMパッケージインストール
"/mnt/c/Program Files/nodejs/npm.exe" install

# スクリプト実行
"/mnt/c/Program Files/nodejs/node.exe" script.js
```

### Python
```bash
# Pythonバージョン確認
"/mnt/c/Users/username/AppData/Local/Programs/Python/Python311/python.exe" --version

# スクリプト実行
"/mnt/c/Users/username/AppData/Local/Programs/Python/Python311/python.exe" script.py
```

### Git (Windows版)
```bash
# Gitバージョン確認
"/mnt/c/Program Files/Git/cmd/git.exe" --version

# ステータス確認
"/mnt/c/Program Files/Git/cmd/git.exe" status
```

### PowerShell
```bash
# PowerShellコマンド実行
"/mnt/c/Windows/System32/WindowsPowerShell/v1.0/powershell.exe" -Command "Get-Date"

# PowerShellスクリプト実行
"/mnt/c/Windows/System32/WindowsPowerShell/v1.0/powershell.exe" -File "script.ps1"
```

## 実行可能ファイルの検索方法

### 1. Windows側でのパス確認
```cmd
# Windows コマンドプロンプトで実行
where dotnet
where node
where python
```

### 2. WSL2側でのファイル検索
```bash
# 特定のファイルを検索
find /mnt/c -name "dotnet.exe" 2>/dev/null

# Program Files内を検索
find "/mnt/c/Program Files" -name "*.exe" -type f 2>/dev/null | grep -i dotnet
```

### 3. 実行可能性の確認
```bash
# ファイルの存在確認
ls -la "/mnt/c/Program Files/dotnet/dotnet.exe"

# ファイルタイプ確認
file "/mnt/c/Program Files/dotnet/dotnet.exe"

# 実行権限確認
test -x "/mnt/c/Program Files/dotnet/dotnet.exe" && echo "実行可能" || echo "実行不可"
```

## 高度な実行テクニック

### 1. 環境変数の設定
```bash
# Windows側の環境変数を設定して実行
DOTNET_CLI_TELEMETRY_OPTOUT=1 "/mnt/c/Program Files/dotnet/dotnet.exe" --version
```

### 2. 作業ディレクトリの指定
```bash
# 特定のディレクトリで実行
cd /mnt/c/Users/username/project
"/mnt/c/Program Files/dotnet/dotnet.exe" build
```

### 3. 出力のリダイレクト
```bash
# 標準出力をファイルに保存
"/mnt/c/Program Files/dotnet/dotnet.exe" --version > dotnet_version.txt

# エラー出力も含めて保存
"/mnt/c/Program Files/dotnet/dotnet.exe" build 2>&1 | tee build_log.txt
```

### 4. 複数コマンドの連携
```bash
# 複数のコマンドを順次実行
"/mnt/c/Program Files/dotnet/dotnet.exe" build && "/mnt/c/Program Files/dotnet/dotnet.exe" test

# 結果を変数に格納
BUILD_RESULT=$("/mnt/c/Program Files/dotnet/dotnet.exe" build 2>&1)
echo "Build result: $BUILD_RESULT"
```

## エラーハンドリング

### 1. 一般的なエラーと対処法

#### "command not found" エラー
```bash
# 原因: パスが間違っている
# 対処: ファイルの存在確認
ls -la "/mnt/c/Program Files/dotnet/dotnet.exe"
```

#### "Permission denied" エラー
```bash
# 原因: 実行権限がない
# 対処: 権限確認と付与
chmod +x "/mnt/c/Program Files/dotnet/dotnet.exe"
```

#### "No such file or directory" エラー
```bash
# 原因: パスにスペースが含まれているが引用符がない
# 対処: 引用符で囲む
"/mnt/c/Program Files/dotnet/dotnet.exe"
```

### 2. デバッグ用コマンド
```bash
# 実行トレースを有効にする
set -x
"/mnt/c/Program Files/dotnet/dotnet.exe" --version
set +x

# 詳細なエラー情報を表示
"/mnt/c/Program Files/dotnet/dotnet.exe" --version 2>&1 | cat -v
```

## 自動化とスクリプト化

### 1. 関数化
```bash
# ~/.bashrc に追加
function run_dotnet() {
    "/mnt/c/Program Files/dotnet/dotnet.exe" "$@"
}

# 使用例
run_dotnet --version
run_dotnet build
```

### 2. エイリアス設定
```bash
# ~/.bashrc に追加
alias dotnet='"/mnt/c/Program Files/dotnet/dotnet.exe"'
alias winpython='"/mnt/c/Users/username/AppData/Local/Programs/Python/Python311/python.exe"'

# 使用例
dotnet --version
winpython script.py
```

### 3. スクリプト例
```bash
#!/bin/bash
# deploy.sh

set -e  # エラー時に終了

echo "Building project..."
"/mnt/c/Program Files/dotnet/dotnet.exe" build

echo "Running tests..."
"/mnt/c/Program Files/dotnet/dotnet.exe" test --verbosity quiet

echo "Deployment completed successfully!"
```

## 注意事項

### 1. パフォーマンス
- WSL2からWindows実行ファイルを呼び出すとオーバーヘッドが発生
- 頻繁に実行する場合は、Windows側で直接実行することを検討

### 2. 文字エンコーディング
- Windows側の出力が文字化けする場合がある
- 必要に応じて文字エンコーディングを指定

### 3. パス区切り文字
- Windows: `\` (バックスラッシュ)
- Linux: `/` (スラッシュ)
- WSL2では Linux 形式を使用

### 4. 環境変数の違い
- Windows側とWSL2側で環境変数が異なる場合がある
- 必要に応じて明示的に設定

## 活用場面

### 1. 開発環境
- Linux環境で開発、Windows側のビルドツールを使用
- クロスプラットフォーム開発での統合環境構築

### 2. CI/CD
- WSL2環境での自動化スクリプト
- Windows特有のツールとLinuxツールの連携

### 3. 既存ツールの活用
- Windows側の既存ツールをLinux環境から利用
- ライセンスの関係でWindows版しか使えないツールの活用

---

**対象環境**: WSL2 (Ubuntu/Debian/その他Linux) + Windows 10/11  
**最終更新**: 2025年7月11日