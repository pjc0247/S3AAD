S3AAD
====

use `aws.s3` as a database for straightforward services.

__initialization__
```cs
S3DB.Init("ACCESS_KEY", "ACCESS_SECRET", "ap-northeast-2");
```

__create document__
```cs
var document = S3DB.CreateDocument("my-bucket", "username");
```

__save document__
```cs
document["level"] = 1234;
document["nickname"] = "eel";

document.Update();
```

__find document__
```cs
var document = S3DB.FindDocument("my-bucket", "username");

var level = (int)document["level"];
var nickname = (string)document["nickname"];
```

* there's no API provided to delete a document. 


Concurrency
----
TODO
