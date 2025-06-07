#!/bin/bash

host="$1"
shift
cmd="$@"

echo "⏳ Waiting for PostgreSQL at $host:5432..."

until nc -z "$host" 5432; do
  echo "Waiting for PostgreSQL..."
  sleep 2
done

echo "✅ PostgreSQL is up - starting application"
exec $cmd
