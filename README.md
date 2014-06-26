Tokenken2
=========

http://ham007.hatenablog.com/entry/2014/06/20/002510 の調査時に作成したものです


## プロジェクト構成

+ 管理者登録サイトプロジェクト : Tokenken2

+ WebAPIプロジェクト : Tokenken2Api

+ Webアプリケーションプロジェクト : Tokenken2App


## 利用するまでの手順

1. 管理者登録サイトプロジェクトとWebAPIプロジェクトのWeb.configを編集する。 appSettingsのtokenDbConnectionのvalueを"＜接続文字列＞"からトークン生成秘密キー保持するDBへの接続文字列に変更する。

1. 管理者登録サイトプロジェクトを実行して、トークン発行を行う。 詳細手順は トークン発行手順 を参照。

1. WebAPIプロジェクトのWeb.configを編集する。 appSettingsのcurrentTokenのvalueを"＜トークン値＞"から発行したトークンに差し替える。ここまでの手順でWebアプリケーションがWebAPIを使用するまでの準備が完了した。

1. WebAPIプロジェクトとWebアプリケーションプロジェクトを実行する。


## トークン発行手順

1. https://localhost:44301/login にアクセスしてログイン。 (ユーザー名:user , パスワード:password)

1. https://localhost:44301/secure にリダイレクトされるので、トークン発行リンククリックする。

1. リンク先には生成されたトークンが表示される。のでテキストエディタなどにコピーしておく。
