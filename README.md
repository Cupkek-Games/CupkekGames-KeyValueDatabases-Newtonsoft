# CupkekGames KeyValueDatabases Newtonsoft

Newtonsoft bridge for [`com.cupkekgames.keyvaluedatabases`](../com.cupkekgames.keyvaluedatabases). Keeps the
core `KeyValueDatabase<TKey,TValue>` serializer-agnostic — the core package has **no** Newtonsoft
dependency — while letting it round-trip through the Newtonsoft save system.

## What's in it

- **`KeyValueDatabaseConverter`** — a `JsonConverter` that (de)serializes any `KeyValueDatabase<,>` as its
  `Pairs` list. It handles the open generic, so every closed type is covered without per-type registration.
- **`KeyValueDatabasesSerializationTypeProviderSO`** — a `SerializationTypeProviderSO` that supplies the
  converter to the `SerializationManager`.

## Setup

1. Create a *KeyValueDatabase Type Provider* asset
   (`Create → CupkekGames → Data → Newtonsoft → KeyValueDatabase Type Provider`).
2. Add it to your `SerializationManagerRegistrar`'s providers list (alongside the Data type provider).

That's it — `KeyValueDatabase<,>` (and anything containing one, e.g. an `InventoryWithSlots`) now persists
through Newtonsoft.

## Why a separate package

Unity serialization is built into the core type (`[SerializeField]` + lazy cache) because the engine has no
pluggable converters. Third-party serializers *do*, so each one's glue lives in its own optional bridge —
the same pattern as `CupkekGames.Data.Newtonsoft`. Don't need Newtonsoft? Don't install this; the core
package still works under Unity serialization.
