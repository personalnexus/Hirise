# Hirise
Hirise is my experimental _**Hi**erarchical **R**ed**i**s **Se**rver_.

Multiple users can connect to it and load and store items in a folder hierarchy. Each item maintains meta data about its last store.

## Namespaces
- T: Tree
- U: User

### Tree: Folders and Items

Each item has a name and is stored in a folder which in turn can be stored in another folder.
Each folder has a name.
Each folder has a path derived from the names of its parents.
Each item has a path derived from the path of its parent folder and its own name.

Items can contain string data, or a string representation of one or more other folder or item paths. The tree class will resolve them to actual item and folder objects on access.

Currently, only items are saved in the database as folders (currently) do not store any data of their own. This will change when folder hiearchy based access-control is added.

### Users

The user subsystem is almost completely non-existant. Only basic data about a users last connection is stored. This will change when access control is added.

## To do

- pub/sub when item data changes
- access control
- fewer wasteful string operations