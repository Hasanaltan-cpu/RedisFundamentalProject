<h2>Redis : <h2> 
Redis is an open source (BSD licensed), in-memory data structure store used as a database, cache, message broker, and streaming engine. Redis provides data structures such as strings, hashes, lists, sets, sorted sets with range queries, bitmaps, hyperloglogs, geospatial indexes, and streams.
Managing Databases. Out of the box, a Redis instance supports 16 logical databases. These databases are effectively siloed off from one another, and when you run a command in one database it doesn't affect any of the data stored in other databases in your Redis instance.


 For first installation image on the docker. You should exec this command to cli.
  
  
docker run --name my-redis -p 6379:6379 -d redis
  
  If u have this image on your local , it won t take pull again.
  
 *  Route port 6379 on my laptop to port 6379 inside the container. 6379 is Redis default port and can be changed
  
  ➜ docker stop my-redis
  
    my-redis
  
  ➜ redis-cli
  
    Could not connect to Redis at 127.0.0.1:6379: Connection refused
    not connected> exit
  
    ➜ docker start my-redis
  
    my-redis
  
    ➜ redis-cli
  
    127.0.0.1:6379> get name
    "Monica"
    127.0.0.1:6379>
  
  
  *For monitoring u can use Another Redis Desktop with configuration.
