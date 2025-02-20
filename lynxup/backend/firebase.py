import firebase_admin
from firebase_admin import credentials, firestore, db

# Load service account key JSON
cred = credentials.Certificate("lynxup_key.json")
firebase_admin.initialize_app(cred)

db_firestore = firestore.client()

# Test code:

# # Add data
# doc_ref = db_firestore.collection("users").document("user1")
# doc_ref.set({"name": "Alice", "age": 25})
#
# # Read data
# doc = db_firestore.collection("users").document("user1").get()
# print(doc.to_dict())

