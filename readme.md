S3AAD
====

use `aws.s3` as a database for straightforward services.

<br>

__Initialization__
```cs
S3DB.Init("ACCESS_KEY", "ACCESS_SECRET", "ap-northeast-2");
```
`IAM` 설정에서 읽기 전용 토큰을 발급하는것으로 권한을 조절할 수 있습니다.

__CREATE__
```cs
var document = S3DB.CreateDocument("my-bucket", "username");
```

__UPDATE__
```cs
document["level"] = 1234;
document["nickname"] = "eel";

document.Update();
```

__FIND__
```cs
var document = S3DB.FindDocument("my-bucket", "username");

var level = (int)document["level"];
var nickname = (string)document["nickname"];
```

* there's no API provided to delete a document. 

limitations
----
* No query filters (only supports exact key based matching)
* Can only fetch 1 document at once
* No atomic operations and locks
* Cannot delete documents

concurrency
----
* 단일 document에 대해서 쓰기 작업은 원자성을 보장합니다. 작업은 항상 전부 반영되거나, 전부 반영되지 않습니다.
