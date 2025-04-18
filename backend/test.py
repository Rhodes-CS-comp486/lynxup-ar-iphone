import firebase_admin
from firebase_admin import credentials, firestore

cred = credentials.Certificate("lynxup_key.json")
firebase_admin.initialize_app(cred)

db = firestore.client()
db.collection("test").document("ping").set({"message": "hello"})
print("Success")
