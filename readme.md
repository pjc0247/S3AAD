S3AAD
====

use `aws.s3` as a database for straightforward services.

<br>
__initialization__
```cs
S3DB.Init("ACCESS_KEY", "ACCESS_SECRET", "ap-northeast-2");
```

__create a document__
```cs
var document = S3DB.CreateDocument("my-bucket", "username");
```

__save a document__
```cs
document["level"] = 1234;
document["nickname"] = "eel";

document.Update();
```

__find a single document__
```cs
var document = S3DB.FindDocument("my-bucket", "username");

var level = (int)document["level"];
var nickname = (string)document["nickname"];
```

* there's no API provided to delete a document. 

Limitations
----
* No query filters (only supports exact key based matching)
* Can fetch only 1 document at once
* No atomic operations and locks
* Cannot delete documents

Concurrency
----
* 단일 document에 대해서 쓰기 작업은 원자성을 보장합니다. 작업은 항상 전부 반영되거나, 전부 반영되지 않습니다.
