/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, 
such modified software should be clearly marked, so as not to confuse it with the version available from LANL.

Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided 
that the following conditions are met:
•	Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
disclaimer. 
•	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
disclaimer in the documentation and/or other materials provided with the distribution. 
•	Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, 
nor the names of its contributors may be used to endorse or promote products derived from this software without specific 
prior written permission. 
THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using DetectorDefs;
using NCCReporter;
namespace AnalysisDefs
{

    using NC = NCC.CentralizedState;


    public interface IDetAPI<T>
    {
        /// <summary>
        /// Examine DB for existence of data object of type T
        /// </summary>
        /// <param name="det">Detector associated with this data object, can be null</param>
        /// <returns>true if found, false on error or not found</returns>
        bool Has(Detector det, T val);

        bool Has(string detname, T val);

        /// <summary>
        /// Get the singleton data object of type T, optionally get that associated with Detector det
        /// </summary>
        /// <param name="det">Detector associated with this data object, can be null</param>
        /// <returns>T, the data object of type T associated with det, null if an error occurred or none found</returns>
        T Get(Detector det);

        T Get(Predicate<T> match);

        T Get(string detname);

        /// <summary>
        /// Get the List of all data objects of type T, optionally get those associated with Detector det
        /// </summary>
        /// <param name="det">Detector associated with this data object, can be null</param>
        /// <returns>The possibly empty list of T, the data object of type T associated with det</returns>
        List<T> GetList(Detector det);

        List<T> GetList(string detname);

        Dictionary<Detector, T> GetMap();

        Dictionary<Detector, T> Map {  get; }

        /// <summary>
        /// Create (or update existing) new data object of type T, optionally associated with Detector det
        /// </summary>
        /// <param name="val">data object for inserting or updating to DB</param>
        /// <param name="det">Detector associated with this data object, can be null</param>
        /// <returns>Unique id of data object (created or  updated). -1 on error</returns>
        long Set(Detector det, T val);

        /// <summary>
        /// Create (or update existing) new data object of type T, optionally associated with Detector det
        /// </summary>
        /// <param name="val">data object for inserting or updating to DB</param>
        /// <param name="detname">Detector associated with this data object, can be null</param>
        /// <returns>Unique id of data object (created or  updated). -1 on error</returns>
        long Set(string detname, T val);

        /// <summary>
        /// Create (or update existing) new data objects of type T, with detectors in an associative map, a dictionary
        /// </summary>
        /// <param name="val">List of data objects for inserting or updating to DB. If null, use the internally implemented collection</param>
        /// <returns>Unique id of last data object (created or  updated). -1 on error</returns>
        long SetMap();

        /// <summary>
        /// Add data object of type T to database, optionally associated with Detector det
        /// Also adds to in-memory map
        /// </summary>
        /// <param name="val">data object to Add to DB</param>
        /// <param name="det">Detector associated with this data object, can be null</param>
        /// <returns>true if added, false if not added, or on error</returns>
        bool Add(Detector det, T val);

        /// <summary>
        /// Update data object of type T on in-memory map and in database
        /// </summary>
        /// <param name="det">The unique detector, the map key</param>
        /// <param name="val">data object to update</param>
        /// <returns>true if updated, false if not found or on error</returns>
        bool Update(Detector det, T val);

        /// <summary>
        /// Delete the map entry for the detector, and delete the corresponding database row
        /// </summary>
        /// <param name="det">The unique detector, the map key</param>
        /// <returns>true if map and database entries deleted</returns>
        bool Delete(Detector det);

    }

    public interface IAPI<T>
    {
        /// <summary>
        /// Examine DB for existence of data object of type T
        /// </summary>
        /// <param name="val">object to look for</param>
        /// <returns>true if found, false on error or not found</returns>
        bool Has(T val);

        /// <summary>
        /// Examine DB for existence of data object of type T
        /// </summary>
        /// <param name="name">object id or 'name' for which one searches for</param>
        /// <returns>true if found, false on error or not found</returns>
        bool Has(string name);

        /// <summary>
        /// Get the data object of type T that matches using a comparison delegate
        /// </summary>
        /// <param name="match">predicate delegate used to match on elements in DB, See List<T>.Find for more info</param>
        /// <returns>T, the data object of type T associated with det, null if an error occurred or none found</returns>
        T Get(Predicate<T> match);

        /// <summary>
        /// Get the data object of type T that matches a name 
        /// </summary>
        /// <param name="name">matches the first 'name' of a specific type T</param>
        /// <returns>T, the data object of type T associated with det, null if an error occurred or none found</returns>
        T Get(string name);

        /// <summary>
        /// Get the singleton data object of type T, optionally get that associated with Detector det
        /// </summary>
        /// <returns>T, the data object of type T associated with det, null if an error occurred or none found</returns>
        T Get();

        /// <summary>
        /// Get the List of all data objects of type T, optionally get those associated with Detector det
        /// </summary>
        /// <returns>The possibly empty list of T, the data object of type T associated with det</returns>
        List<T> GetList();

        /// <summary>
        /// Create (or update existing) new data object of type T, optionally associated with Detector det
        /// </summary>
        /// <param name="val">data object for inserting or updating to DB</param>
         /// <returns>Unique id of data object (created or  updated). -1 on error</returns>
        long Set(T val);

        /// <summary>
        /// Create (or update existing) new data objects of type T, optionally associated with Detector det
        /// </summary>
        /// <param name="val">List of data objects for inserting or updating to DB. If null, use the internally implemented collection</param>
         /// <returns>Unique id of last data object (created or  updated). -1 on error</returns>
        long SetList(List<T> vals = null);

        /// <summary>
        /// Delete data object of type T, identified by name or by complete object
        /// </summary>
        /// <param name="name">name of o complete data object to delete from DB</param>
        /// <returns>true if deleted, false if not found or on error</returns>
        bool Delete(string name);
        bool Delete(T val);

        /// <summary>
        /// Delete data objects of type T, optionally associated with Detector det
        /// </summary>
        /// <param name="val">List of data objects to delete from DB</param>
         /// <returns>true if all deleted, false if one or more not found, or on error</returns>
        bool Delete(List<T> vals);
    }

    public class IsotopicsListImpl : IAPI<Isotopics>
    {
        // the isotopics list 
        List<Isotopics> isotopics;

        public IsotopicsListImpl()
        {
            isotopics = null;
        }

        public bool Has(Isotopics iso)
        {
            return null != GetList().Find(il => { return iso.CompareTo(il) == 0; });
        }
        public bool Has(string name)
        {
            return null != GetList().Find(il => { return string.Compare(name, il.id, StringComparison.OrdinalIgnoreCase) == 0; });
        }

        public List<Isotopics> GetMatch(Predicate<Isotopics> match)
        {
            return GetList().FindAll(match);
        }

        public Isotopics Get(string name)
        {
            return GetList().Find(il => { return string.Compare(name, il.id, StringComparison.OrdinalIgnoreCase) == 0; });
        }
        public Isotopics Get(Predicate<Isotopics> match)
        {
            return GetList().Find(match);
        }

        public Isotopics GetDefault()
        {
            Isotopics iso = Get(Isotopics.DefaultId);
            if (iso == null)
            {
                iso = new Isotopics();
                GetList().Add(iso);
                Set(iso);
            }
            return iso;
        }

        public static Isotopics GetIsotopicsByRow(DataRow dr, bool resultsSubset = false)
        {
            Isotopics iso = new Isotopics();
            foreach (ValueType v in System.Enum.GetValues(typeof(Isotope)))
            {
                if (dr.Table.Columns.IndexOf(v.ToString()) >= 0)
                    iso.SetValueError((Isotope)v, DB.Utils.DBDouble(dr[v.ToString()]), DB.Utils.DBDouble(dr[v.ToString() + "_err"]));
            }
            iso.pu_date = DB.Utils.DBDateTime(dr["pu_date"]);
            iso.am_date = DB.Utils.DBDateTime(dr["am_date"]);
            System.Enum.TryParse<Isotopics.SourceCode>(dr["isotopics_source_code"].ToString(), out  iso.source_code);
            iso.id = dr["isotopics_id"].ToString();
            return iso;
        }

		/// <summary>
		/// Load an in-memory list from the database
		/// </summary>
		/// <returns>List of Isotopics</returns>
        public List<Isotopics> GetList()
        {
            if (isotopics == null)
            {
                isotopics = new List<Isotopics>();
                DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.Isotopics);
                foreach (DataRow dr in dt.Rows)
                {
                    Isotopics iso = GetIsotopicsByRow(dr);
                    isotopics.Add(iso);
                }
                if (isotopics.Count < 1) // if the list was not loaded in the foreach above
                    isotopics.Add(new Isotopics()); // add the default isotopics instance
            }
            return isotopics;
        }

        public Isotopics Get()
        {
             return null;
        }

		/// <summary>
        /// Force subsequent list request to refresh directly from the database
        /// </summary>
        public void Refresh()
        {
			isotopics = null;
			GetList();
        }

        public void Replace(Isotopics riso)
        {
            Isotopics i = Get(riso.id);
            riso.CopyTo(i);
        }
        private DB.Isotopics isodb;

		/// <summary>
		/// Insert or update an isotopics definition in the database
		/// </summary>
		/// <param name="iso">The Isotopics instance</param>
		/// <returns>The unique database key for the record</returns>
        public long Set(Isotopics iso)
        {
            long id = -1;
            if (iso.modified)
            {
                if (isodb == null)
                    isodb = new DB.Isotopics();
                id = isodb.Update(iso.id, iso.ToDBElementList());
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34037, "Updated or created an Isotopics Id {0} ({1})", iso.id, id);
                if (id >= 0) iso.modified = false;                
            }
            return id;
        }

        public long SetList(List<Isotopics> vals = null)
        {
            isodb = new DB.Isotopics();
            //WTF?  If you send in a list, actually use those values......hn 3.17.2015
            if (vals != null)
            {
                long updated = 0;
                foreach (Isotopics i in vals)
                {
                    try
                    {
                        updated += isodb.Update(i.id, i.ToDBElementList());
                    }
                    catch (Exception e)
                    {
                        NC.App.Pest.logger.TraceEvent(LogLevels.Error, 34044, "Iso update punted out.");
                        NC.App.Pest.logger.TraceException(e, false);
                    }
                }
                isotopics = vals;
                return updated;
            }
            else // an empty list implies writing the in-memory list to the database 
            {
                long res = 0;
                if (isotopics == null)
                {
                    int count = GetList().Count;
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34036, "{0} isotopics read initially from DB", count);
					return res; // nothing to write
                }               

                try
                {
                    foreach (Isotopics iso in isotopics)
                    {
                        if (iso.modified)
                        {
                            res = isodb.Update(iso.id, iso.ToDBElementList());
                            NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34037, "Updated or created isotopics {0} ({1})", iso.id, res);
                            if (res >= 0) 
							{
								res++; iso.modified = false;
							}
                        }
                    }
                }
                catch (Exception e)
                {
                    NC.App.Pest.logger.TraceEvent(LogLevels.Error, 34044, "Iso update punted out.");
                    NC.App.Pest.logger.TraceException(e, false);
                }
                return res;
            }
        }

		/// <summary>
		/// Delete an isotopics from the database and the in-memory list
		/// </summary>
		/// <param name="iso">The isotopics instance subject to deletion</param>
		/// <returns>true iff deleted</returns>
        public bool Delete(Isotopics iso)
        {
            DB.Isotopics isodb = new DB.Isotopics();
            if (isodb.Delete(iso.id))
                return isotopics.Remove(iso);
            else
                return false;
        }

		/// <summary>
		/// Delete an isotopics from the database and the in-memory list
		/// </summary>
		/// <param name="iso">The isotopics instance subject to deletion</param>
		/// <returns>true iff deleted</returns>
        public bool Delete(string name)
        {
           Isotopics i= Get(name);
           return Delete(i);
        }


		/// <summary>
		/// Delete a a list of isotopics from the database and the in-memory list
		/// </summary>
		/// <param name="itol">The list</param>
		/// <returns>true iff all deleted</returns>
        public bool Delete(List<Isotopics> itol)
        {
            bool res = false;
            DB.Isotopics itodb = new DB.Isotopics();
            foreach (Isotopics iso in itol)
            {
                if (itodb.Delete(iso.id))
                    res = res && isotopics.Remove(iso);
                else
                    res = false;
            }
            return res;
        }

		/// <summary>
		///  Change the "id" (name) of an isotopics in the database 
		/// </summary>
		/// <param name="old">existing "id"</param>
		/// <param name="NewId">new "id"</param>
		/// <returns></returns>
        public bool Rename(string OldId, string NewId)
        {
            DB.Isotopics isodb = new DB.Isotopics();
            return isodb.Update(isodb.PrimaryKey(OldId), NewId);
        }


        /// <summary>
        /// Revert this isotopics on in-memory list back to DB values
        /// </summary>
        /// <param name="iso">The isotopics instance subject to reversion</param>
        /// <returns>true iff reverted</returns>
        public bool Revert(Isotopics iso)
        {
            DB.Isotopics isodb = new DB.Isotopics();
            DataTable dt = isodb.GetRows(iso.id);
            if (dt.Rows.Count > 0)
            {
                DataRow drl = dt.Rows[0];
                Isotopics liso = GetIsotopicsByRow(drl);
                iso.Copy(liso);
                iso.modified = false;
                return true;
            }
            return false;
        }

        
    }
    public class CompositeIsotopicsListImpl : IAPI<CompositeIsotopics>
    {
        // the isotopics table 
        List<CompositeIsotopics> comp_isotopics;

        public CompositeIsotopicsListImpl()
        {
            comp_isotopics = null;
        }

        public bool Has(CompositeIsotopics iso)
        {
            return null != GetList().Find(il => { return iso.CompareTo(il) == 0; });
        }
        public bool Has(string name)
        {
            return null != GetList().Find(il => { return name.CompareTo(il.id) == 0; });
        }
        public CompositeIsotopics Get(string name)
        {
            return GetList().Find(il => { return name.CompareTo(il.id) == 0; });
        }
        public CompositeIsotopics Get(Predicate<CompositeIsotopics> match)
        {
            return GetList().Find(match);
        }

        public List<CompositeIsotopics> GetList()
        {
            if (comp_isotopics == null)
            {
                comp_isotopics = new List<CompositeIsotopics>();
                DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.CompositeIsotopics);
                foreach (DataRow dr in dt.Rows)
                {
                    CompositeIsotopics iso = new CompositeIsotopics();
                    foreach (ValueType v in System.Enum.GetValues(typeof(Isotope)))
                    {
                        if (dt.Columns.IndexOf(v.ToString()) >= 0)
                            iso.SetVal((Isotope)v, DB.Utils.DBDouble(dr[v.ToString()]));
                    }
                    iso.pu_date = DB.Utils.DBDateTime(dr["ci_pu_date"]);
                    iso.am_date = DB.Utils.DBDateTime(dr["ci_am_date"]);
                    iso.ref_date = DB.Utils.DBDateTime(dr["ci_ref_date"]);
                    System.Enum.TryParse(dr["ci_isotopics_source_code"].ToString(), out iso.source_code);
                    iso.id = dr["ci_isotopics_id"].ToString();
                    comp_isotopics.Add(iso);
                }
                if (comp_isotopics.Count < 1)
                    comp_isotopics.Add(new CompositeIsotopics());
            }
            return comp_isotopics;
        }

        public CompositeIsotopics Get()
        {
            return null;
        }

        public void Replace(CompositeIsotopics riso)
        {
            CompositeIsotopics i = Get(riso.id);
            riso.CopyTo(i);
        }
        private DB.CompositeIsotopics compisodb;
        public long Set(CompositeIsotopics iso)
        {
            //todo: see if this really works
            long success = -1;
            if (iso.modified)
            {
                if (compisodb == null)
                    compisodb = new DB.CompositeIsotopics();
                if (iso.modified)
                {
                    success = compisodb.Update(iso.id, iso.ToDBElementList());
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34037, "Updated or created an Isotopics Id {0} ({1})", iso.id, success);
                    if (success >= 0) iso.modified = false;
                }
            }
            return success;
        }

        public long SetList(List<CompositeIsotopics> vals = null)
        {
            long res = -1;
            if (comp_isotopics == null)
            {
                int count = GetList().Count;
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34036, "{0} composite isotopics read initially from DB", count);
            }
            DB.CompositeIsotopics comp_isodb = new DB.CompositeIsotopics();

            try
            {
                foreach (CompositeIsotopics iso in comp_isotopics)
                {
                    if (iso.modified)
                    {
                        res = comp_isodb.Update(iso.id, iso.ToDBElementList());
                        NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34037, "Updated or created composite isotopics {0} ({1})", iso.id, res);
                        if (res >= 0) iso.modified = false;
                    }
                }
            }
            catch (Exception e)
            {
                NC.App.Pest.logger.TraceEvent(LogLevels.Error, 34044, "Composite Iso update punted out.");
                NC.App.Pest.logger.TraceException(e, false);
            }
            return res;
        }

        public bool Delete(CompositeIsotopics iso)
        {
            DB.CompositeIsotopics comp_isodb = new DB.CompositeIsotopics();
            if (comp_isodb.Delete(iso.id))
                return comp_isotopics.Remove(iso);
            else
                return false;
        }

        public bool Delete(string name)
        {
            CompositeIsotopics i = Get(name);
            return Delete(i);
        }

        public bool Delete(List<CompositeIsotopics> itol)
        {
            bool res = false;
            DB.CompositeIsotopics itodb = new DB.CompositeIsotopics();
            foreach (CompositeIsotopics iso in itol)
            {
                if (itodb.Delete(iso.id))
                    res = res && comp_isotopics.Remove(iso);
                else
                    res = false;
            }
            return res;
        }

    }
    public class CollarItemIdListImpl : IAPI<CollarItemId>
    {
        public CollarItemIdListImpl()
        {
        }

        public bool Has(CollarItemId item)
        {
            return Has(item.item_id);
        }

        public bool Has(string item)
        {
            return null != GetList().Find(d => string.Compare(d.item_id, item, true) == 0);
        }

        public CollarItemId Get(Predicate<CollarItemId> match)
        {
            return GetList().Find(match);
        }

        public CollarItemId Get(string name)
        {
            return GetList().Find(d => string.Compare(d.item_id, name, true) == 0);
        }

        //  the item table 
        List<CollarItemId> items;

        public List<CollarItemId> GetList()
        {
            //if (items == null)
            //{
            items = new List<CollarItemId>();
            DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.CollarItems);
            foreach (DataRow dr in dt.Rows)
            {
                CollarItemId ito = new CollarItemId();
                ito.item_id = dr["item_name"].ToString();
                ito.rod_type = dr["rod_type"].ToString();
                ito.total_rods = DB.Utils.DBDouble(dr["total_rods"].ToString());
                ito.total_poison_rods = DB.Utils.DBDouble(dr["total_poison_rods"].ToString());
                ito.length = VTupleHelper.Make(dr, "length_entry");
                ito.total_pu = VTupleHelper.Make(dr, "total_pu");
                ito.depleted_u = VTupleHelper.Make(dr, "depleted_u");
                ito.natural_u = VTupleHelper.Make(dr, "natural_u");
                ito.enriched_u = VTupleHelper.Make(dr, "natural_u");
                ito.total_u235 = VTupleHelper.Make(dr, "total_u235");
                ito.total_u238 = VTupleHelper.Make(dr, "total_u238");
                ito.poison_percent.v = DB.Utils.DBDouble(dr["poison_percent"]);

                items.Add(ito);
            }
            // }
            return items;
        }

        public CollarItemId Get()
        {
            return null;
        }
        private DB.CollarItems itodb;
        public long Set(CollarItemId ito)
        {
            long id = -1;
            if (ito.modified)
            {
                if (itodb == null)
                    itodb = new DB.CollarItems();
                id = itodb.Update(ito.item_id, ito.ToDBElementList());
                ito.modified = false;
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34025, "Updated/created CollarItem Id {0} {1}", ito.item_id, id);
            }
            return id;
        }

        public long SetList(List<CollarItemId> vals = null)
        {
            long res = -1;
            if (items == null)
            {
                int count = GetList().Count;
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34024, "{0}  items read initially from DB", count);
            }
            if (itodb == null)
                itodb = new DB.CollarItems();
            try
            {
                foreach (CollarItemId ito in items)
                {
                    res = Set(ito);
                }
            }
            catch (Exception e)
            {
                NC.App.Pest.logger.TraceEvent(LogLevels.Error, 34023, "Item update punted out.");
                NC.App.Pest.logger.TraceException(e, false);
            }
            return res;
        }

        public bool Delete(CollarItemId ito)
        {
            DB.CollarItems itodb = new DB.CollarItems();
            if (itodb.Delete(ito.item_id))
                return items.Remove(ito);
            else
                return false;
        }

        public bool Delete(string name)
        {
            CollarItemId i = Get(name);
            return Delete(i);
        }

        public bool Delete(List<CollarItemId> itol)
        {
            bool res = false;
            DB.CollarItems itodb = new DB.CollarItems();
            foreach (CollarItemId ito in itol)
            {
                if (itodb.Delete(ito.item_id))
                    res = res && items.Remove(ito);
                else
                    res = false;
            }
            return res;
        }

        // Delete ItemIds from the database based on a list of ItemId name strings
        public bool Delete(List<string> names)
        {
            bool res = false;   // Default condition is failure
            DB.CollarItems itodb = new DB.CollarItems();

            // Grab each string from the list and operate separately
            foreach (string name in names)
            {
                // Is there an item with this name?
                if (itodb.Has(name))
                {
                    // Grab a temporary copy of the item in question
                    CollarItemId temp = Get(name);

                    // Try to delete the item from the DB
                    if (itodb.Delete(name))
                        // If the delete operation succeeded and it was the only name on the list, return true
                        res = res && items.Remove(temp);
                    else
                        // If the delete failed or there are more items to try, return false
                        res = false;
                }
                else
                    // There was no item with this name; result is false for this round
                    res = false;
            }
            return res;
        }

        public bool Rename(string old, string NewId)
        {
            DB.CollarItems itodb = new DB.CollarItems();
            return itodb.Update(itodb.PrimaryKey(old), NewId);
        }
    }

    public class ItemIdListImpl : IAPI<ItemId>
    {

        public ItemIdListImpl()
        {
        }

        public bool Has(ItemId item)
        {
            return Has(item.item);
        }

        public bool Has(string item)
        {
            return null != GetList().Find(d => string.Compare(d.item, item, true) == 0);
        }

        public ItemId Get(Predicate<ItemId> match)
        {
            return GetList().Find(match);
        }

        public ItemId Get(string name)
        {
            return GetList().Find(d => string.Compare(d.item, name, true) == 0);
        }

        //  the item table 
        List<ItemId> items;
        public static ItemId GetItemIdByRow(DataRow dr, bool resultsSubset)
        {
            ItemId ito = new ItemId();
            if (resultsSubset)
            {
                ito.item = dr["item_id"].ToString();
                ito.material = string.Empty;
                ito.isotopics = dr["isotopics_id"].ToString();
                ito.stratum = dr["stratum_id"].ToString();
                ito.inventoryChangeCode = dr["inventory_change_code"].ToString();
                ito.IOCode = dr["io_code"].ToString();
                ito.declaredMass = DB.Utils.DBDouble(dr["declared_mass"].ToString());
                ito.declaredUMass = DB.Utils.DBDouble(dr["declared_u_mass"].ToString());
                ito.length = DB.Utils.DBDouble(dr["length"].ToString());
                ito.mba = dr["mba"].ToString();
                ito.am_date = DB.Utils.DBDateTime(dr["am_date"]);
                ito.pu_date = DB.Utils.DBDateTime(dr["pu_date"]);
            }
            else
            {
                ito.item = dr["item_name"].ToString();
                ito.material = dr["material_type_id"].ToString();
                ito.isotopics = dr["isotopics_id"].ToString();
                ito.stratum = dr["stratum_id"].ToString();
                ito.inventoryChangeCode = dr["inventory_change_code"].ToString();
                ito.IOCode = dr["io_code"].ToString();
                ito.declaredMass = DB.Utils.DBDouble(dr["declared_mass_entry"].ToString());
                ito.declaredUMass = DB.Utils.DBDouble(dr["declared_u_mass_entry"].ToString());
                ito.length = DB.Utils.DBDouble(dr["length_entry"].ToString());
                ito.mba = dr["mba"].ToString();
            }
            return ito;
        }
        public List<ItemId> GetList()
        {
            if (items == null)
            {
                items = new List<ItemId>();
                DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.Items);
                foreach (DataRow dr in dt.Rows)
                {
                    ItemId ito = GetItemIdByRow(dr,false);
                    items.Add(ito);
                }
            }
            return items;
        }
        public List<ItemId> GetListByStratumID(string value)
        {
            items = new List<ItemId>();
            DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.Items);
            foreach (DataRow dr in dt.Rows)
            {
                ItemId ito = GetItemIdByRow(dr, false);
                if (ito.stratum == value) // todo: iffy string comparison
                     items.Add(ito);
            }
            return items;
        }
        public ItemId Get()
        {
            return null;
        }
        private DB.Items itodb;
        public long Set(ItemId ito)
        {
            long id = -1;
            if (ito.modified)
            {
                if (itodb == null)
                    itodb = new DB.Items();
                id = itodb.Update(ito.item, ito.ToDBElementList());
                ito.modified = false;
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34025, "Updated or created Item Id {0} ({1})", ito.item, id);
            }
            return id;
        }
        public void Set()
        {
            GetList();
            foreach (ItemId itid in items)
            {
                Set(itid);
            }
        }
        public long SetList(List<ItemId> vals = null)
        {
            long res = -1;
            if (items == null)
            {
                int count = GetList().Count;
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34024, "{0}  items read initially from DB", count);
            }
            if (itodb == null)
                itodb = new DB.Items();
            try
            {
                foreach (ItemId ito in items)
                {
                    res = Set(ito);
                }
            }
            catch (Exception e)
            {
                NC.App.Pest.logger.TraceEvent(LogLevels.Error, 34023, "Item update punted out.");
                NC.App.Pest.logger.TraceException(e, false);
            }
            return res;
        }

        public bool Delete(ItemId ito)
        {
            DB.Items itodb = new DB.Items();
            if (itodb.Delete(ito.item))
                return items.Remove(ito);
            else
                return false;
        }

        public bool Delete(string name)
        {
            ItemId i = Get(name);
            return Delete(i);
        }

        public bool Delete(List<ItemId> itol)
        {
            bool res = false;
            DB.Items itodb = new DB.Items();
            foreach (ItemId ito in itol)
            {
                if (itodb.Delete(ito.item))
                    res = res && items.Remove(ito);
                else
                    res = false;
            }
            return res;
        }

        // Delete ItemIds from the database based on a list of ItemId name strings
        public bool Delete(List<string> names)
        {
            bool res = false;   // Default condition is failure
            DB.Items itodb = new DB.Items();

            // Grab each string from the list and operate separately
            foreach (string name in names)
            {
                // Is there an item with this name?
                if (itodb.Has(name))
                {
                    // Grab a temporary copy of the item in question
                    ItemId temp = Get(name);

                    // Try to delete the item from the DB
                    if (itodb.Delete(name))
                        // If the delete operation succeeded and it was the only name on the list, return true
                        res = res && items.Remove(temp);
                    else
                        // If the delete failed or there are more items to try, return false
                        res = false;
                }
                else
                    // There was no item with this name; result is false for this round
                    res = false;
            }
            return res;
        }

        public bool Rename(string old, string NewId)
        {
            DB.Items itodb = new DB.Items();
            return itodb.Update(itodb.PrimaryKey(old), NewId);
        }

		
        public void Refresh()
        {
			items = null;
			GetList();
        }
    }

    public class TestParamsImpl : IAPI<TestParameters>
    {

        public TestParamsImpl()
        {
        }

        //current test params, not attached to anything
        BindingList<TestParameters> testParameters;

        /// <summary>
        /// Eventhandler that updates the DB with any changes to the list of TestParaemters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TestParametersSet_ListChanged(object sender, ListChangedEventArgs e)
        {
            // update database
            DB.TestParams t = new DB.TestParams();  // gets DB connection to table

            DB.ElementList saParams;
            if (GetAll() != null && GetAll().Count > 0)
            {
                saParams = GetAll().Last().ToDBElementList();
                bool there = t.Has();
                if (!there) // test not there, so add it
                {
                    long l = t.Create(saParams);
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34005, INCCDB.MakeIdFrag(l) + " new test params");
                }
                else
                {
                    bool b = t.Update(saParams);
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34005, INCCDB.UpdateFrag(b) + " test params");
                }
            }
        }

        /// <summary>
        /// Are these exact test parameters already defined?
        /// </summary>
        /// <param name="tp">The parameters sought</param>
        /// <returns>True if at least one is a deep match</returns>
        public bool Has(TestParameters tp)
        {
            foreach (TestParameters ltp in GetList())
            {
                if (tp.CompareTo(ltp) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// True if at least one is available
        /// </summary>
        /// <param name="name">ignored</param>
        /// <returns></returns>
        public bool Has(string name)
        {
            return GetList().Count > 0;
        }

        /// <summary>
        /// Return the most recently modified or updated test parameters
        /// </summary>
        /// <returns>null if no TestParameters exists  o.w. the newest is returned</returns>
        public TestParameters Get()
        {
            if (GetList().Count > 0)
                return GetList().Last();
            else
                return null;
        }

        /// <summary>
        /// Return the most recently modified or updated test parameters
        /// </summary>
        /// <param name="name">ignored</param>
        /// <returns>null if no TestParameters exists  o.w. the newest is returned</returns>
        public TestParameters Get(string name)
        {
            if (GetList().Count > 0)
                return GetList().Last();
            else
                return null;
        }

        public TestParameters Get(Predicate<TestParameters> match)
        {
            foreach (TestParameters ltp in GetList())
            {
                if (match(ltp))
                    return ltp;
            }
            return null;
        }

        static public TestParameters GetFromDataRow(DataRow dr)
        {

            TestParameters tp = new TestParameters();

            tp.accSnglTestRateLimit = DB.Utils.DBDouble(dr["acc_sngl_test_rate_limit"].ToString());
            tp.accSnglTestPrecisionLimit = DB.Utils.DBDouble(dr["acc_sngl_test_precision_limit"].ToString());
            tp.accSnglTestOutlierLimit = DB.Utils.DBDouble(dr["acc_sngl_test_outlier_limit"].ToString());
            tp.outlierTestLimit = DB.Utils.DBDouble(dr["outlier_test_limit"].ToString());
            tp.bkgDoublesRateLimit = DB.Utils.DBDouble(dr["bkg_doubles_rate_limit"].ToString());
            tp.bkgTriplesRateLimit = DB.Utils.DBDouble(dr["bkg_triples_rate_limit"].ToString());
            tp.chiSquaredLimit = DB.Utils.DBDouble(dr["chisq_limit"].ToString());
            tp.maxNumFailures = DB.Utils.DBUInt16(dr["max_num_failures"].ToString());
            tp.normalBackupAssayTestLimit = DB.Utils.DBDouble(dr["normal_backup_assay_test_limit"].ToString());
            tp.highVoltageTestLimit = DB.Utils.DBDouble(dr["high_voltage_test_limit"].ToString());
            tp.maxCyclesForOutlierTest = DB.Utils.DBUInt32(dr["max_runs_for_outlier_test"].ToString());
            tp.checksum = DB.Utils.DBBool(dr["checksum_test"].ToString());

            System.Enum.TryParse<AccidentalsMethod>(dr["accidentals_method"].ToString(), out tp.accidentalsMethod);

            return tp;
        }


        public List<TestParameters> GetList()
        {
            object o = GetAll();  // force load of local binding list
            return testParameters.ToList<TestParameters>();
        }

        BindingList<TestParameters> GetAll()
        {
            if (testParameters == null)
            {
                testParameters = new BindingList<TestParameters>();

                DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.TestParams);
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        TestParameters tp = GetFromDataRow(dr);
                        testParameters.Add(tp);
                    }
                    catch (Exception e)
                    {
                        NC.App.Pest.logger.TraceException(e, false);
                    }
                }
                testParameters.ListChanged += new ListChangedEventHandler(TestParametersSet_ListChanged);
            }
            return testParameters;
        }



        public long SetList(List<TestParameters> vals = null)
        {
            long res = -1;
            if (testParameters == null)
            {
                int count = GetList().Count;  // force DB read
            }
            return res;
        }
        public long Set(TestParameters val)
        {
            long res = -1;
            if (testParameters == null)
            {
                int count = GetList().Count;  // force DB read
            }
			// if it is already there don't add it
			foreach (TestParameters tp in testParameters)
			{
				if (tp.CompareTo(val) == 0)
					res = 0;
					break;
			}
			if (res < 0) 
			{ 
				testParameters.Add(val);
				testParameters = null;
				int count = GetList().Count;  // force DB read
				res = 0;
			}
            return res;
        }         

        public bool Delete(TestParameters tp)
        {
            DB.TestParams db = new DB.TestParams();
            if (db.Delete(""))
                return testParameters.Remove(tp);
            else
                return false;
        }
        public bool Delete(string name)
        {
            TestParameters t = Get(name);
            return Delete(t);
        }
        public bool Delete(List<TestParameters> itol)
        {
            bool res = false;
            return res;
        }
    }

    public class  cm_pu_ratio_Impl : IAPI<INCCAnalysisParams.cm_pu_ratio_rec>
    {

        public cm_pu_ratio_Impl()
        {
        }

        //current cm_pu_ratio, not attached to anything
        BindingList<INCCAnalysisParams.cm_pu_ratio_rec> cm_pu_ratio;

        /// <summary>
        /// Eventhandler that updates the DB with any changes to the list of cm_pu_ratio_rec
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cm_pu_ratioSet_ListChanged(object sender, ListChangedEventArgs e)
        {
            // update database
            DB.cm_pu_ratio_rec t = new DB.cm_pu_ratio_rec();  // gets DB connection to table

            DB.ElementList saParams;
            if (GetAll() != null && GetAll().Count > 0)
            {
                saParams = GetAll().Last().ToDBElementList();
                bool there = t.Has();
                if (!there) // item not there, so add it
                {
                    long l = t.Create(saParams);
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34005, INCCDB.MakeIdFrag(l) + " new cm_pu_ratio params");
                }
                else
                {
                    bool b = t.Update(saParams);
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34005, INCCDB.UpdateFrag(b) + " cm_pu_ratio params");
                }
            }
        }

        /// <summary>
        /// Are these exact cm_pu_ratio_rec already defined?
        /// </summary>
        /// <param name="tp">The parameters sought</param>
        /// <returns>True if at least one is a deep match</returns>
        public bool Has(INCCAnalysisParams.cm_pu_ratio_rec tp)
        {
            foreach (INCCAnalysisParams.cm_pu_ratio_rec ltp in GetList())
            {
                if (tp.CompareTo(ltp) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// True if at least one is available
        /// </summary>
        /// <param name="name">ignored</param>
        /// <returns></returns>
        public bool Has(string name)
        {
            return GetList().Count > 0;
        }

        /// <summary>
        /// Return the most recently modified or updated INCCAnalysisParams.cm_pu_ratio_rec parameters
        /// </summary>
        /// <returns>null if no INCCAnalysisParams.cm_pu_ratio_rec exists  o.w. the newest is returned</returns>
        public INCCAnalysisParams.cm_pu_ratio_rec Get()
        {
            if (GetList().Count > 0)
                return GetList().Last();
            else
                return null;
        }

        /// <summary>
        /// Return the most recently modified or updated INCCAnalysisParams.cm_pu_ratio_rec parameters
        /// </summary>
        /// <param name="name">ignored</param>
        /// <returns>null if no INCCAnalysisParams.cm_pu_ratio_rec exists  o.w. the newest is returned</returns>
        public INCCAnalysisParams.cm_pu_ratio_rec Get(string name)
        {
            if (GetList().Count > 0)
                return GetList().Last();
            else
                return null;
        }

        public INCCAnalysisParams.cm_pu_ratio_rec Get(Predicate<INCCAnalysisParams.cm_pu_ratio_rec> match)
        {
            foreach (INCCAnalysisParams.cm_pu_ratio_rec ltp in GetList())
            {
                if (match(ltp))
                    return ltp;
            }
            return null;
        }


        public List<INCCAnalysisParams.cm_pu_ratio_rec> GetList()
        {
            object o = GetAll();  // force load of local binding list
            return cm_pu_ratio.ToList<INCCAnalysisParams.cm_pu_ratio_rec>();
        }

        BindingList<INCCAnalysisParams.cm_pu_ratio_rec> GetAll()
        {
            if (cm_pu_ratio == null)
            {
                cm_pu_ratio = new BindingList<INCCAnalysisParams.cm_pu_ratio_rec>();

                DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.TestParams);
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        INCCAnalysisParams.cm_pu_ratio_rec v = new INCCAnalysisParams.cm_pu_ratio_rec();
                        cm_pu_ratio.Add(v);
                        v.cm_pu_ratio.v = DB.Utils.DBDouble(dr["cm_pu_ratio"]);
                        v.cm_pu_ratio.err = DB.Utils.DBDouble(dr["cm_pu_ratio_err"]);
                        v.cm_u_ratio.v = DB.Utils.DBDouble(dr["cm_u_ratio"]);
                        v.cm_u_ratio.err = DB.Utils.DBDouble(dr["cm_u_ratio_err"]);
                        v.cm_pu_half_life = DB.Utils.DBDouble(dr["pu_half_life"]);
                        v.cm_pu_ratio_date = DB.Utils.DBDateTime(dr["cm_pu_ratio_date"]);
                        v.cm_id_label = dr["cm_id_label"].ToString();
                        v.cm_id = dr["cm_id"].ToString();
                        v.cm_input_batch_id = dr["cm_input_batch_id"].ToString();
                        v.cm_dcl_u_mass = DB.Utils.DBDouble(dr["dcl_u_mass"]);
                        v.cm_dcl_u235_mass = DB.Utils.DBDouble(dr["dcl_u235_mass"]);
;
                    }
                    catch (Exception e)
                    {
                        NC.App.Pest.logger.TraceException(e, false);
                    }
                }
                cm_pu_ratio.ListChanged += new ListChangedEventHandler(cm_pu_ratioSet_ListChanged);
            }
            return cm_pu_ratio;
        }



        public long SetList(List<INCCAnalysisParams.cm_pu_ratio_rec> vals = null)
        {
            long res = -1;
            if (cm_pu_ratio == null)
            {
                int count = GetList().Count;  // force DB read
            }
            return res;
        }
        public long Set(INCCAnalysisParams.cm_pu_ratio_rec val)
        {
            long res = -1;
            if (cm_pu_ratio == null)
            {
                int count = GetList().Count;  // force DB read
            }
            cm_pu_ratio.Add(val);
            return res;
        }

        // todo: this likely does not actually remove it from DB and list, so test it
        public bool Delete(INCCAnalysisParams.cm_pu_ratio_rec tp)
        {
            DB.cm_pu_ratio_rec db = new DB.cm_pu_ratio_rec();
            if (db.Delete(""))
                return cm_pu_ratio.Remove(tp);
            else
                return false;
        }
        public bool Delete(string name)
        {
            INCCAnalysisParams.cm_pu_ratio_rec t = Get(name);
            return Delete(t);
        }
        public bool Delete(List<INCCAnalysisParams.cm_pu_ratio_rec> itol)
        {
            bool res = false;
            return res;
        }
    }




    public class NormParamsImpl : IDetAPI<NormParameters>
    {
        public bool Has(Detector det, NormParameters val)
        {
            DB.NormParams npdb = new DB.NormParams();
            bool there = npdb.Has(det.Id.DetectorId);
            return there;
        }
        public bool Has(string detname, NormParameters val)
        {
            DB.NormParams npdb = new DB.NormParams();
            bool there = npdb.Has(detname);
            return there;
        }

        public NormParameters Get(Detector det)
        {
            return Get(det.Id.DetectorId);
        }
        public NormParameters Get(string detname)
        {
            Dictionary<Detector, NormParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    return iter.Current.Value;
            }
            return null;
        }

        public NormParameters Get(Predicate<NormParameters> match)
        {
            return null;
        }

        public List<NormParameters> GetList(Detector det)
        {
            List < NormParameters > l =  new List<NormParameters>();
            l.Add(GetMap()[det]);
            return l;
        }
        public List<NormParameters> GetList(string detname)
        {
            List<NormParameters> l = new List<NormParameters>();
            Dictionary<Detector, NormParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    l.Add(iter.Current.Value);
            }
            return l;
        }

        
        Dictionary<Detector, NormParameters> normParameters;
        public static NormParameters GetFromDataRow(DataRow dr, bool resultsSubset)
        {
            NormParameters np = new NormParameters();

            np.currNormalizationConstant = VTupleHelper.Make(dr, "normalization_constant"); 
            
            if (resultsSubset)
                return np;
            else
                np.sourceId = dr["source_id"].ToString();
                np.biasMode = (NormTest)(DB.Utils.DBInt32(dr["bias_mode"]));
                np.measRate = VTupleHelper.Make(dr, "meas_rate");
                np.amliRefSinglesRate = DB.Utils.DBDouble(dr["amli_ref_singles_rate"]);
                np.cf252RefDoublesRate = VTupleHelper.Make(dr, "cf252_ref_doubles_rate");
                np.refDate = DB.Utils.DBDateTime(dr["ref_date"]);
                np.initSrcPrecisionLimit = DB.Utils.DBDouble(dr["init_src_precision_limit"]);
                np.biasPrecisionLimit = DB.Utils.DBDouble(dr["bias_precision_limit"]);
                np.acceptanceLimitStdDev = DB.Utils.DBDouble(dr["acceptance_limit_std_dev"]);
                np.acceptanceLimitPercent = DB.Utils.DBDouble(dr["acceptance_limit_percent"]);
                np.yieldRelativeToMrc95 = DB.Utils.DBDouble(dr["yield_relative_to_mrc_95"]);
                np.biasTestUseAddasrc = DB.Utils.DBBool(dr["bias_test_use_addasrc"]);
                np.biasTestAddasrcPosition = DB.Utils.DBDouble(dr["bias_test_addasrc_position"]);

            return np;
        }

        public Dictionary<Detector, NormParameters> Map
        {
            get { return GetMap();  }
        }
        public Dictionary<Detector, NormParameters> GetMap()
        {
            if (normParameters == null)
            {
                normParameters = new Dictionary<Detector, NormParameters>();
                foreach (Detector d in NC.App.DB.Detectors)
                {
                    DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.NormParams, d.Id.DetectorId);

                    foreach (DataRow dr in dt.Rows)
                    try
                    {
                        NormParameters np = GetFromDataRow(dr,false);
                        normParameters.Add(d, np);
                    }
                    catch (Exception e)
                    {
                        NC.App.Pest.logger.TraceException(e, false);
                    }
                }
            }
            return normParameters;
        }


        /// <summary>
        /// create/update each norm found on the map
        /// </summary>
        /// <returns>id of last accessed norm in DB</returns>
       public long SetMap()
       {
           long l = -1;
           Dictionary<Detector, NormParameters>.Enumerator iter = GetMap().GetEnumerator();
           while (iter.MoveNext())
                Set (iter.Current.Key, iter.Current.Value);
           return l;
       }


       /// <summary>
       /// adds val to DB
       /// </summary>
       /// <param name="val"></param>
       /// <param name="det">El detecto</param>
       /// <returns></returns>
       public long Set(Detector det, NormParameters val)
       {
           return Set(det.Id.DetectorId, val);
       }


        /// <summary>
        /// adds val to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="det">Detector superior</param>
        /// <returns></returns>
       public long Set(string detname, NormParameters np)
        {
            long res = -1;

            DB.ElementList saParams;
            saParams = np.ToDBElementList();
            DB.NormParams npdb = new DB.NormParams();
            bool there = npdb.Has(detname);
            if (!there) // item not there, so add it
            {
                bool b = npdb.Create(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34005, INCCDB.MakeFrag(b) + " new norm param state for {0}", detname);
                if (b) res = 0;
            }
            else
            {
                bool b = npdb.Update(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34005, INCCDB.UpdateFrag(b) + " norm param state for {0}", detname);
                if (b) res = 0;
            }
            return res;
        }

        public bool Add(Detector det, NormParameters val)
        {
            Dictionary<Detector, NormParameters> m = GetMap();
            m.Add(det, val);
            long l = Set(det, val);
            return l >= 0;
        }

        public bool Update(Detector det, NormParameters val)
        {
            Dictionary<Detector, NormParameters> m = GetMap();
            if (m.ContainsKey(det)) m[det] = val;
            DB.NormParams db = new DB.NormParams();
            bool there = db.Has(det.Id.DetectorId);
            if (there)
                there = db.Update(det.Id.DetectorId, val.ToDBElementList());
            return there;
        }

        /// <summary>
        /// Delete the map entry for the detector, and delete the corresponding database row
        /// </summary>
        /// <param name="det"></param>
        /// <returns>true if map and database entries deleted</returns>
        public bool Delete(Detector det)
        {
            bool b = Map.Remove(det);
            if (b)
            {
                DB.NormParams db = new DB.NormParams();
                bool there = db.Has(det.Id.DetectorId);
                if (there)
                    b = db.Delete(det.Id.DetectorId);
            }
            return b;
        }

    }


    public class ResultsRecs
    {
        // map from detector name to general INCC5 results record
        Dictionary<string, List<INCCResults.results_rec>> results;
        public ResultsRecs()
        {
            results = new Dictionary<string, List<INCCResults.results_rec>>();
        }


        public INCCResults.results_rec GetResultsFor(string detname, MeasId mid)
        {
			INCCResults.results_rec rec = Get(mid.UniqueId); 
            return rec;
        }

        public List<INCCResults.results_rec> GetResultsFor(string detname)
        {
            DB.Results r = new DB.Results();
            DataTable dt = r.ResultsForDet(detname);
            List<INCCResults.results_rec> res = new List<INCCResults.results_rec>();
            string curr_det = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                res.Add(RowParser(dr, ref curr_det));
            }   
            return res;
        }


        public INCCResults.results_rec Get(long mid)
		{
			DB.Results r = new DB.Results();
			DataTable dt = r.Result(mid);
			INCCResults.results_rec rr = null;
			string curr_det = string.Empty;
			foreach (DataRow dr in dt.Rows)
            {
				rr = RowParser(dr, ref curr_det);
				break; // take the first (and only) one
			}
			return rr;
		}


		INCCResults.results_rec RowParser(DataRow dr, ref string curr_det)
		{
            INCCResults.results_rec resrec = new INCCResults.results_rec();
            // reconstruct the acquire params used for this measurement result
            resrec.meas_option = AssaySelectorExtensions.SrcToEnum(dr["meas_option"].ToString());
            resrec.det = INCCDB.GetDetectorParmsFromDataRow(dr, resultsSubset: true);
            resrec.acq = INCCDB.GetAcquireParmsFromDataRow(ref curr_det, dr, resultsSubset: true, isLM: resrec.det.ListMode);
            resrec.tests = TestParamsImpl.GetFromDataRow(dr);
            resrec.norm = NormParamsImpl.GetFromDataRow(dr, resultsSubset: true);  // dev note: examine the reconstruction code in INCC5 to know if one must blend these values with current normalization settings for this detector?
            resrec.bkg = BackgroundParamsImpl.GetDataFromRow(dr, resultsSubset: true);
            resrec.st = INCCDB.GetStratumByRow(dr, resultsSubset: true);

            // item id
            resrec.item = ItemIdListImpl.GetItemIdByRow(dr, resultsSubset: true);
            resrec.item.material = string.Copy(resrec.acq.item_type);

            // reconstruct the isotopics used for the measurement results               
            resrec.iso = IsotopicsListImpl.GetIsotopicsByRow(dr);

            resrec.mcr = new MultiplicityCountingRes();
            resrec.mcr.Totals = DB.Utils.DBDouble(dr["singles_sum"]);
            resrec.mcr.S1Sum = DB.Utils.DBDouble(dr["scaler1_sum"]);
            resrec.mcr.S2Sum = DB.Utils.DBDouble(dr["scaler2_sum"]);
            resrec.mcr.RASum = DB.Utils.DBDouble(dr["reals_plus_acc_sum"]);
            resrec.mcr.ASum = DB.Utils.DBDouble(dr["acc_sum"]);
            resrec.mcr.RAMult = DB.Utils.ReifyUInt64s(dr["mult_reals_plus_acc_sum"].ToString());
            resrec.mcr.NormedAMult = DB.Utils.ReifyUInt64s(dr["mult_acc_sum"].ToString());
            resrec.mcr.DeadtimeCorrectedRates.Singles = VTupleHelper.Make(dr, "singles");
            resrec.mcr.DeadtimeCorrectedRates.Doubles = VTupleHelper.Make(dr, "doubles");
            resrec.mcr.DeadtimeCorrectedRates.Triples = VTupleHelper.Make(dr, "triples");
            resrec.mcr.Scaler1 = VTupleHelper.Make(dr, "scaler1");
            resrec.mcr.Scaler2 = VTupleHelper.Make(dr, "scaler2");
            resrec.mcr.rates.RawRates.Doubles = VTupleHelper.Make(dr, "uncorrected_doubles");
            resrec.mcr.singles_multi = DB.Utils.DBDouble(dr["singles_multi"]);
            resrec.mcr.doubles_multi = DB.Utils.DBDouble(dr["doubles_multi"]);
            resrec.mcr.triples_multi = DB.Utils.DBDouble(dr["triples_multi"]);
            resrec.mcr.Mass = DB.Utils.DBDouble(dr["declared_mass"]);

            System.Enum.TryParse<AnalysisMethod>(dr["primary_analysis_method"].ToString(), out resrec.primary);
            resrec.total_number_runs = DB.Utils.DBInt32(dr["total_number_runs"]);
            resrec.total_good_count_time = DB.Utils.DBDouble(dr["total_good_count_time"]);
            resrec.net_drum_weight = DB.Utils.DBDouble(dr["net_drum_weight"]);
            resrec.db_version = DB.Utils.DBDouble(dr["db_version"]);
            resrec.completed = DB.Utils.DBBool(dr["completed"]);

            resrec.MeasId = DB.Utils.DBInt32(dr["mid"]);
            DB.Measurements m = new DB.Measurements();
            DataTable mt = m.Measurement(resrec.MeasId);
            // for now, get the timestamp from the measurements table and place it on the Acquire params instance
            // NEXT: the result_rec should have one, two or three timestamps, "original_meas_date", and the "passive_DateTime" and the "active_DateTime", but these have not yet been worked out correctly
            // Should only be one measurement.....
            foreach (DataRow dm in mt.Rows)
            {
                DateTimeOffset dto = DB.Utils.DBDateTimeOffset(dm["DateTime"]);
                resrec.acq.MeasDateTime = dto;
                break;
            }

			return resrec;
		}
    }

    public static class VTupleHelper
    {
        /// <summary>
        /// Helper function to create a VTuple from a database table (value + sigma)
        /// Assumes the INCC DB convention where the "_err" suffix denotes the the sigma column entry
        /// </summary>
        /// <param name="dr">The DataRow from a previous DB query</param>
        /// <param name="v">The table column name</param>
        /// <returns>A new VTUple wth teh vlaues</returns>
        public static AnalysisDefs.VTuple Make(DataRow dr, string v)
        {
            return VTuple.Create(DB.Utils.DBDouble(dr[v]), DB.Utils.DBDouble(dr[v + "_err"]));
        }
    }

    public class UnattendedParamsImpl : IDetAPI<UnattendedParameters>
    {
        public bool Has(Detector det, UnattendedParameters val)
        {
            DB.UnattendParams npdb = new DB.UnattendParams();
            bool there = npdb.Has(det.Id.DetectorId);
            return there;
        }
        public bool Has(string detname, UnattendedParameters val)
        {
            DB.UnattendParams npdb = new DB.UnattendParams();
            bool there = npdb.Has(detname);
            return there;
        }
        public UnattendedParameters Get(Detector det)
        {
            return Get(det.Id.DetectorId);
        }
        public UnattendedParameters Get(string detname)
        {
            Dictionary<Detector, UnattendedParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    return iter.Current.Value;
            }
            return null;
        }
        public UnattendedParameters Get(Predicate<UnattendedParameters> match)
        {
            return null;
        }

        public List<UnattendedParameters> GetList(Detector det)
        {
            List<UnattendedParameters> l = new List<UnattendedParameters>();
            l.Add(GetMap()[det]);
            return l;
        }
        public List<UnattendedParameters> GetList(string detname)
        {
            List<UnattendedParameters> l = new List<UnattendedParameters>();
            Dictionary<Detector, UnattendedParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    l.Add(iter.Current.Value);
            }
            return l;
        }

        Dictionary<Detector, UnattendedParameters> unattParameters;

        public Dictionary<Detector, UnattendedParameters> Map
        {
            get { return GetMap(); }
        }
        public Dictionary<Detector, UnattendedParameters> GetMap()
        {
            if (unattParameters == null)
            {
                unattParameters = new Dictionary<Detector, UnattendedParameters>();
                foreach (Detector d in NC.App.DB.Detectors)
                {
                    DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.UnattendedParams, d.Id.DetectorId);
                    foreach (DataRow dr in dt.Rows)
                    {
                        UnattendedParameters bp = new UnattendedParameters();
                        bp.ErrorSeconds = DB.Utils.DBUInt32(dr["error_seconds"]);
                        bp.AutoImport = DB.Utils.DBBool(dr["auto_import"]);
                        bp.AASThreshold = DB.Utils.DBDouble(dr["add_a_source_threshold"]);
                        unattParameters.Add(d, bp);
                    }
                }
            }
            return unattParameters;
        }


        /// <summary>
        /// create/update each norm found on the map
        /// </summary>
        /// <returns>id of last accessed norm in DB</returns>
       public long SetMap()
       {
           long l = -1;
           Dictionary<Detector, UnattendedParameters>.Enumerator iter = GetMap().GetEnumerator();
           while (iter.MoveNext())
                Set (iter.Current.Key, iter.Current.Value);
           return l;
       }

       /// <summary>
       /// adds val to DB
       /// </summary>
       /// <param name="val"></param>
       /// <param name="detname">The detector</param>
       /// <returns></returns>
        public long Set(Detector det, UnattendedParameters val)
        {
            return Set(det.Id.DetectorId, val);
        }

        /// <summary>
        /// adds val to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="detname">name of detector</param>
        /// <returns></returns>
       public long Set(string detname, UnattendedParameters val)
        {
            long res = -1;
            DB.ElementList saParams;
            saParams = val.ToDBElementList();
            DB.UnattendParams updb = new DB.UnattendParams();
            bool there = updb.Has(detname); // todo: use overwrite flag
            if (!there) // item not there, so add it
            {
                bool b = updb.Create(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34027, INCCDB.MakeFrag(b) + " new unattended param state for {0}", detname);
                if (b) res = 0;
            }
            else
            {
                bool b = updb.Update(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34028, INCCDB.UpdateFrag(b) + " unattended param state for {0}", detname);
                if (b) res = 0;
            }
            return res;
        }

        public bool Add(Detector det, UnattendedParameters val)
        {
            Dictionary<Detector, UnattendedParameters> m = GetMap();
            m.Add(det, val);
            long l = Set(det, val);
            return l >= 0;
        }


        /// <summary>
        /// Update data object on in-memory map and in database
        /// </summary>
        /// <param name="det">The unique detector, the map key</param>
        /// <param name="val">data object to update</param>
        /// <returns>true if updated, false if not found or on error</returns>
        public bool Update(Detector det, UnattendedParameters val)
        {
            Dictionary<Detector, UnattendedParameters> m = GetMap();
            if (m.ContainsKey(det)) m[det] = val;
            DB.UnattendParams db = new DB.UnattendParams();
            bool there = db.Has(det.Id.DetectorId);
            if (there)
                there = db.Update(det.Id.DetectorId, val.ToDBElementList());
            return there;
        }

        /// <summary>
        /// Delete the map entry for the detector, and delete the corresponding database row
        /// </summary>
        /// <param name="det"></param>
        /// <returns>true if map and database entries deleted</returns>
        public bool Delete(Detector det)
        {
            bool b = Map.Remove(det);
            if (b)
            {
                DB.UnattendParams npdb = new DB.UnattendParams();
                bool there = npdb.Has(det.Id.DetectorId);
                if (there)
                    b = npdb.Delete(det.Id.DetectorId);
            }
            return b;
        }

    }

    public class BackgroundParamsImpl : IDetAPI<BackgroundParameters>
    {
        public bool Has(Detector det, BackgroundParameters val)
        {
            DB.BackgroundParams bdb = new DB.BackgroundParams();
            bool there = bdb.Has(det.Id.DetectorId);
            return there;
        }
        public bool Has(string detname, BackgroundParameters val)
        {
            DB.BackgroundParams bdb = new DB.BackgroundParams();
            bool there = bdb.Has(detname);
            return there;
        }
        public BackgroundParameters Get(Detector det)
        {
            return Get(det.Id.DetectorId);
        }
        public BackgroundParameters Get(string detname)
        {
            Dictionary<Detector, BackgroundParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    return iter.Current.Value;
            }
            return null;
        }
        public BackgroundParameters Get(Predicate<BackgroundParameters> match)
        {
            Dictionary<Detector, BackgroundParameters>.Enumerator iter = GetMap().GetEnumerator();
            return null;// GetMap().Find(match);
        }

        public List<BackgroundParameters> GetList(Detector det)
        {
            List<BackgroundParameters> l = new List<BackgroundParameters>();
            l.Add(GetMap()[det]);
            return l;
        }
        public List<BackgroundParameters> GetList(string detname)
        {
            List<BackgroundParameters> l = new List<BackgroundParameters>();
            Dictionary<Detector, BackgroundParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    l.Add(iter.Current.Value);
            }
            return l;
        }

        Dictionary<Detector, BackgroundParameters> bkgParameters;
        public static BackgroundParameters GetDataFromRow(DataRow dr, bool resultsSubset)
        {
            // Modify to add doubles/triples for active Cf252 measurements.  DB has been modified.
            // HN 8.19.2015
            BackgroundParameters bp = new BackgroundParameters();
            bp.DeadtimeCorrectedSinglesRate.v = DB.Utils.DBDouble(dr["passive_bkg_singles_rate"]);
            bp.DeadtimeCorrectedSinglesRate.err = DB.Utils.DBDouble(dr["passive_bkg_singles_rate_err"]);
            bp.DeadtimeCorrectedDoublesRate.v = DB.Utils.DBDouble(dr["passive_bkg_doubles_rate"]);
            bp.DeadtimeCorrectedDoublesRate.err = DB.Utils.DBDouble(dr["passive_bkg_doubles_rate_err"]);
            bp.DeadtimeCorrectedTriplesRate.v = DB.Utils.DBDouble(dr["passive_bkg_triples_rate"]);
            bp.DeadtimeCorrectedTriplesRate.err = DB.Utils.DBDouble(dr["passive_bkg_triples_rate_err"]);
            bp.Scaler1.v = DB.Utils.DBDouble(dr["passive_bkg_scaler1_rate"]);
            bp.Scaler2.v = DB.Utils.DBDouble(dr["passive_bkg_scaler2_rate"]);
            
            bp.INCCActive.Singles = VTupleHelper.Make(dr, "active_bkg_singles_rate");
            bp.INCCActive.Doubles = VTupleHelper.Make(dr, "active_bkg_doubles_rate");
            bp.INCCActive.Triples = VTupleHelper.Make(dr, "active_bkg_triples_rate");
            bp.INCCActive.Scaler1Rate = DB.Utils.DBDouble(dr["active_bkg_scaler1_rate"]);
            bp.INCCActive.Scaler2Rate = DB.Utils.DBDouble(dr["active_bkg_scaler2_rate"]);
            if (resultsSubset)
                return bp;
            bp.TMBkgParams.Singles = VTupleHelper.Make(dr,  "tm_singles_bkg");

            bp.TMBkgParams.Zeros = VTupleHelper.Make(dr, "tm_zeros_bkg");
            bp.TMBkgParams.Ones = VTupleHelper.Make(dr, "tm_ones_bkg");
            bp.TMBkgParams.Twos = VTupleHelper.Make(dr, "tm_twos_bkg");
            bp.TMBkgParams.ComputeTMBkg = DB.Utils.DBBool(dr["tm_bkg"]);
            return bp;
        }

        // compares based only the name
        class EqualityComparer : IEqualityComparer<Detector>
        {
            public EqualityComparer()
            {
            }

            public bool Equals(Detector d1, Detector d2)
            {
                return string.Equals(d1.Id.DetectorName, d2.Id.DetectorName, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(Detector d)
            {
                return d.Id.DetectorName.ToLower().GetHashCode();
            }
        }

        public Dictionary<Detector, BackgroundParameters> Map
        {
            get { return GetMap(); }
        }
        public Dictionary<Detector, BackgroundParameters> GetMap()
        {
            if (bkgParameters == null)
            {
                bkgParameters = new Dictionary<Detector, BackgroundParameters>(new EqualityComparer());
                foreach (Detector d in NC.App.DB.Detectors)
                {
                    DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.BackgroundParams, d.Id.DetectorId);
                    foreach (DataRow dr in dt.Rows)
                    {
                        BackgroundParameters bp = GetDataFromRow(dr, false);
                        bkgParameters.Add(d, bp);
                    }
                }
            }
            return bkgParameters;
        }


        /// <summary>
        /// create/update each bkg found on the map
        /// </summary>
        /// <returns>id of last accessed norm in DB</returns>
        public long SetMap()
        {
            long l = -1;
            Dictionary<Detector, BackgroundParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
                Set(iter.Current.Key, iter.Current.Value);
            return l;
        }

        /// <summary>
        /// adds val to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="det">El detecto</param>
        /// <returns></returns>
        public long Set(Detector det, BackgroundParameters val)
        {
            return Set(det.Id.DetectorId, val);
        }


        /// <summary>
        /// adds val to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="detname">name of detector</param>
        /// <returns></returns>
        public long Set(string detname, BackgroundParameters val)
        {
            DB.BackgroundParams bp = new DB.BackgroundParams();
            DB.ElementList saParams;
            saParams = val.ToDBElementList();
            long res = -1;
            bool bpThere = bp.Has(detname);
            if (!bpThere) // item not there, so add it
            {
                bool b = bp.Create(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34004, INCCDB.MakeFrag(b) + " new bkg param for {0}", detname);
                if (b) res = 0;
            }
            else
            {
                bool b = bp.Update(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34003, INCCDB.UpdateFrag(b) + " bkg param state for {0}", detname);
                if (b) res = 0;
            }
            return res;
        }

        public bool Add(Detector det, BackgroundParameters val)
        {
            Dictionary<Detector, BackgroundParameters> m = GetMap();
            m.Add(det, val);
            long l = Set(det, val);
            return l >= 0;
        }

        /// <summary>
        /// Update data object on in-memory map and in database
        /// </summary>
        /// <param name="det">The unique detector, the map key</param>
        /// <param name="val">data object to update</param>
        /// <returns>true if updated, false if not found or on error</returns>
        public bool Update(Detector det, BackgroundParameters val)
        {
            Dictionary<Detector, BackgroundParameters> m = GetMap();
            if (m.ContainsKey(det)) m[det] = val;
            DB.BackgroundParams db = new DB.BackgroundParams();
            bool there = db.Has(det.Id.DetectorId);
            if (there)
                there = db.Update(det.Id.DetectorId, val.ToDBElementList());
            return there;
        }

        /// <summary>
        /// Delete the map entry for the detector, and delete the corresponding database row
        /// </summary>
        /// <param name="det"></param>
        /// <returns>true if map and database entries deleted</returns>
        public bool Delete(Detector det)
        {
            bool b = Map.Remove(det);
            if (b)
            {
                DB.BackgroundParams db = new DB.BackgroundParams();
                bool there = db.Has(det.Id.DetectorId);
                if (there)
                    b = db.Delete(det.Id.DetectorId);
            }
            return b;
        }

    }

    public class AASParamsImpl : IDetAPI<AddASourceSetup>
    {
        public bool Has(Detector det, AddASourceSetup val)
        {
            DB.AASSetupParams npdb = new DB.AASSetupParams();
            bool there = npdb.Has(det.Id.DetectorId);
            return there;
        }
        public bool Has(string detname, AddASourceSetup val)
        {
            DB.AASSetupParams bdb = new DB.AASSetupParams();
            bool there = bdb.Has(detname);
            return there;
        }
        public AddASourceSetup Get(Detector det)
        {
            return Get(det.Id.DetectorId);
        }
        public AddASourceSetup Get(string detname)
        {
            Dictionary<Detector, AddASourceSetup>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    return iter.Current.Value;
            }
            return null;
        }
        public AddASourceSetup Get(Predicate<AddASourceSetup> match)
        {
            return null;
        }

        public List<AddASourceSetup> GetList(Detector det)
        {
            List<AddASourceSetup> l = new List<AddASourceSetup>();
            l.Add(GetMap()[det]);
            return l;
        }
        public List<AddASourceSetup> GetList(string detname)
        {
            List<AddASourceSetup> l = new List<AddASourceSetup>();
            Dictionary<Detector, AddASourceSetup>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    l.Add(iter.Current.Value);
            }
            return l;
        }

        Dictionary<Detector, AddASourceSetup> aasParameters;
        public Dictionary<Detector, AddASourceSetup> Map
        {
            get { return GetMap(); }
        }

        public Dictionary<Detector, AddASourceSetup> GetMap()
        {
            if (aasParameters == null)
            {
                aasParameters = new Dictionary<Detector, AddASourceSetup>();
                foreach (Detector d in NC.App.DB.Detectors)
                {
                    DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.AASSetupParams, d.Id.DetectorId);
                    foreach (DataRow dr in dt.Rows)
                    {
                        AddASourceSetup aas = new AddASourceSetup();
                        aas.port_number = DB.Utils.DBInt16(dr["port_number"].ToString());
                        AddASourceFlavors a;
                        System.Enum.TryParse<AddASourceFlavors>(dr["type"].ToString(), out  a);
                        aas.type = a;
                        aas.forward_over_travel = DB.Utils.DBDouble(dr["forward_over_travel"].ToString());
                        aas.reverse_over_travel = DB.Utils.DBDouble(dr["reverse_over_travel"].ToString());
                        aas.number_positions = DB.Utils.DBUInt16(dr["number_positions"].ToString());
                        aas.dist_to_move = DB.Utils.ReifyDoubles(dr["dist_to_move"].ToString());
                        aas.cm_forward_mask = DB.Utils.DBUInt32(dr["cm_forward_mask"]);
                        aas.cm_reverse_mask = DB.Utils.DBUInt32(dr["cm_reverse_mask"]);
                        aas.cm_axis_number = DB.Utils.DBInt16(dr["cm_axis_number"]);
                        aas.cm_over_travel_state = DB.Utils.DBUInt32(dr["cm_over_travel_state"]);
                        aas.cm_step_ratio = DB.Utils.DBDouble(dr["cm_step_ratio"]);
                        aas.cm_slow_inches = DB.Utils.DBDouble(dr["cm_slow_inches"]);
                        aas.plc_steps_per_inch = DB.Utils.DBDouble(dr["plc_steps_per_inch"]);
                        aas.scale_conversion_factor = DB.Utils.DBDouble(dr["scale_conversion_factor"]);
                        aas.cm_rotation = DB.Utils.DBBool(dr["cm_rotation"]);

                        aasParameters.Add(d, aas);
                    }

                }
            }
            return aasParameters;
        }


        /// <summary>
        /// create/update each norm found on the map
        /// </summary>
        /// <returns>id of last accessed norm in DB</returns>
        public long SetMap()
        {
            long l = -1;
            Dictionary<Detector, AddASourceSetup>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
                Set(iter.Current.Key, iter.Current.Value);
            return l;
        }

        /// <summary>
        /// adds val to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="det">El detecto</param>
        /// <returns></returns>
        public long Set(Detector det, AddASourceSetup val)
        {
            return Set(det.Id.DetectorId, val);
        }


        /// <summary>
        /// adds val to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="detname">name of detector</param>
        /// <returns></returns>
        public long Set(string detname, AddASourceSetup val)
        {
            DB.AASSetupParams bp = new DB.AASSetupParams();
            DB.ElementList saParams;
            saParams = val.ToDBElementList();
            long res = -1;
            bool bpThere = bp.Has(detname);
            if (!bpThere) // AddASourceSetup not there, so add it
            {
                bool b = bp.Create(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34004, INCCDB.MakeFrag(b) + " new AAS setup param for {0}", detname);
                if (b) res = 0;
            }
            else
            {
                bool b = bp.Update(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34003, INCCDB.UpdateFrag(b) + " AAS setup param state for {0}", detname);
                if (b) res = 0;
            }
            return res;
        }

        public bool Add(Detector det, AddASourceSetup val)
        {
            Dictionary<Detector, AddASourceSetup> m = GetMap();
            m.Add(det, val);
            long l = Set(det, val);
            return l >= 0;
        }


        public bool Update(Detector det, AddASourceSetup val)
        {
            Dictionary<Detector, AddASourceSetup> m = GetMap();
            if (m.ContainsKey(det)) m[det] = val;
            DB.AASSetupParams db = new DB.AASSetupParams();
            bool there = db.Has(det.Id.DetectorId);
            if (there)
                there = db.Update(det.Id.DetectorId, val.ToDBElementList());
            return there;
        }

        /// <summary>
        /// Delete the map entry for the detector, and delete the corresponding database row
        /// </summary>
        /// <param name="det"></param>
        /// <returns>true if map and database entries deleted</returns>
        public bool Delete(Detector det)
        {
            bool b = Map.Remove(det);
            if (b)
            {
                DB.AASSetupParams db = new DB.AASSetupParams();
                bool there = db.Has(det.Id.DetectorId);
                if (there)
                    b = db.Delete(det.Id.DetectorId);
            }
            return b;
        }
    }


    public class HVParamsImpl : IDetAPI<HVCalibrationParameters>
    {
        public bool Has(Detector det, HVCalibrationParameters val)
        {
            DB.HVParams npdb = new DB.HVParams();
            bool there = npdb.Has(det.Id.DetectorId);
            return there;
        }
        public bool Has(string detname, HVCalibrationParameters val)
        {
            DB.HVParams bdb = new DB.HVParams();
            bool there = bdb.Has(detname);
            return there;
        }
        public HVCalibrationParameters Get(Detector det)
        {
            return Get(det.Id.DetectorId);
        }
        public HVCalibrationParameters Get(string detname)
        {
            Dictionary<Detector, HVCalibrationParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    return iter.Current.Value;
            }
            return null;
        }
        public HVCalibrationParameters Get(Predicate<HVCalibrationParameters> match)
        {
            return null;
        }

        public List<HVCalibrationParameters> GetList(Detector det)
        {
            List<HVCalibrationParameters> l = new List<HVCalibrationParameters>();
            l.Add(GetMap()[det]);
            return l;
        }
        public List<HVCalibrationParameters> GetList(string detname)
        {
            List<HVCalibrationParameters> l = new List<HVCalibrationParameters>();
            Dictionary<Detector, HVCalibrationParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Id.DetectorId.Equals(detname, StringComparison.OrdinalIgnoreCase))
                    l.Add(iter.Current.Value);
            }
            return l;
        }

        Dictionary<Detector, HVCalibrationParameters> hvp;

        public Dictionary<Detector, HVCalibrationParameters> Map
        {
            get { return GetMap(); }
        }
        public Dictionary<Detector, HVCalibrationParameters> GetMap()
        {
            if (hvp == null)
            {
                hvp = new Dictionary<Detector, HVCalibrationParameters>();
                foreach (Detector d in NC.App.DB.Detectors)
                {
                    DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.HVParams, d.Id.DetectorId);
                    foreach (DataRow dr in dt.Rows)
                    {
                        HVCalibrationParameters hv = new HVCalibrationParameters();
                        hv.DelayMS = DB.Utils.DBInt32(dr["delay"]);
                        hv.Step = DB.Utils.DBInt32(dr["stepv"]);
                        hv.MaxHV = DB.Utils.DBInt32(dr["maxv"]);
                        hv.MinHV = DB.Utils.DBInt32(dr["minv"]);
                        hv.HVDuration = DB.Utils.DBInt32(dr["duration"]);

                        hvp.Add(d, hv);
                    }

                }
            }
            return hvp;
        }


        /// <summary>
        /// create/update each norm found on the map
        /// </summary>
        /// <returns>id of last accessed norm in DB</returns>
        public long SetMap()
        {
            long l = -1;
            Dictionary<Detector, HVCalibrationParameters>.Enumerator iter = GetMap().GetEnumerator();
            while (iter.MoveNext())
                Set(iter.Current.Key, iter.Current.Value);
            return l;
        }

        /// <summary>
        /// adds val to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="det">El detecto</param>
        /// <returns></returns>
        public long Set(Detector det, HVCalibrationParameters val)
        {
            return Set(det.Id.DetectorId, val);
        }


        /// <summary>
        /// adds val to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="detname">name of detector</param>
        /// <returns></returns>
        public long Set(string detname, HVCalibrationParameters val)
        {
            DB.HVParams bp = new DB.HVParams();
            DB.ElementList saParams;
            saParams = val.ToDBElementList();
            long res = -1;
            bool bpThere = bp.Has(detname);
            if (!bpThere) // not there, so add it
            {
                bool b = bp.Create(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34004, INCCDB.MakeFrag(b) + " new HV param for {0}", detname);
                if (b) res = 0;
            }
            else
            {
                bool b = bp.Update(detname, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34003, INCCDB.UpdateFrag(b) + " HV param state for {0}", detname);
                if (b) res = 0;
            }
            return res;
        }

        public bool Add(Detector det, HVCalibrationParameters val)
        {
            Dictionary<Detector, HVCalibrationParameters> m = GetMap();
            m.Add(det, val);
            long l = Set(det, val);
            return l >= 0;
        }

        /// <summary>
        /// Update data object on in-memory map and in database
        /// </summary>
        /// <param name="det">The unique detector, the map key</param>
        /// <param name="val">data object to update</param>
        /// <returns>true if updated, false if not found or on error</returns>
        public bool Update(Detector det, HVCalibrationParameters val)
        {
            Dictionary<Detector, HVCalibrationParameters> m = GetMap();
            if (m.ContainsKey(det)) m[det] = val;
            DB.HVParams db = new DB.HVParams();
            bool there = db.Has(det.Id.DetectorId);
            if (there)
                there = db.Update(det.Id.DetectorId, val.ToDBElementList());
            return there;
        }

        /// <summary>
        /// Delete the map entry for the detector, and delete the corresponding database row
        /// </summary>
        /// <param name="det"></param>
        /// <returns>true if map and database entries deleted</returns>
        public bool Delete(Detector det)
        {
            bool b = Map.Remove(det);
            if (b)
            {
                DB.HVParams db = new DB.HVParams();
                bool there = db.Has(det.Id.DetectorId);
                if (there)
                    b = db.Delete(det.Id.DetectorId);
            }
            return b;
        }

    }

    /// <summary>
    /// All descriptors (name, description pair) are implemented using this abstract class and it's associated interface.
    /// Implement the constructor by specifying the database enum and the table name, that's all.
    /// All string and INCCDB.Descriptor comparisons are case-insensitive.
    /// </summary>
    public abstract class DescListImpl : IAPI<INCCDB.Descriptor>
    {
        protected DB.Descriptors db;
        protected DB.Pieces el;
        protected string table;
        protected List<INCCDB.Descriptor> descs = null;
        
        public DescListImpl(DB.Pieces e, string tname)
        {
            el = e;
            table = tname;
        }

        public bool Has(INCCDB.Descriptor desc)
        {
            return null != GetList().Find(d => d.CompareTo(desc) == 0);
        }
        public bool Has(string name)
        {
            return null != GetList().Find(d => string.Compare(name, d.Name, true) == 0);
        }
        public INCCDB.Descriptor Get(Predicate<INCCDB.Descriptor> match)
        {
            return GetList().Find(match);
        }
        public INCCDB.Descriptor Get(INCCDB.Descriptor desc)
        {
            return GetList().Find(d => d.CompareTo(desc) == 0);
        }
        public INCCDB.Descriptor Get(string id)
        {
            return GetList().Find(d => string.Compare(id, d.Name, true) == 0);
        }

        public List<INCCDB.Descriptor> GetList()
        {
            if (descs == null)
            {
                descs = new List<INCCDB.Descriptor>();
                DataTableReader dt = NC.App.Pest.GetADataTableReader(el);
                int name = dt.GetOrdinal("name"), descr = dt.GetOrdinal("description");
                while (dt.Read())
                {
                    INCCDB.Descriptor d = new INCCDB.Descriptor(dt.GetString(name), dt.GetString(descr));
                    descs.Add(d);
                }
            }
            return descs;
        }

        /// <summary>
        /// Force subsequent list request to refresh directly from the database
        /// </summary>
        public void Reset()
        {
            descs = null;
        }

        /// <summary>
        /// force an initial DB read
        /// </summary>
        /// <returns></returns>
        public INCCDB.Descriptor Get()
        {
            GetList();
            return null;
        }

        /// <summary>
        /// Update or create a Descriptor on the in-memory list and in the database
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public bool Update(INCCDB.Descriptor desc)
        {
            bool has = Has(desc.Name);  // has this descriptor in the database
            bool b = db.Update(desc.Name, desc.Desc);  // create or update
            long id = db.PrimaryKey(desc.Name);
            if (has)
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34032, INCCDB.UpdateIdFrag(id) + " for " + desc.Name + " in " + table);
            else
            {
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34032, INCCDB.MakeIdFrag(id) + " for " + desc.Name + " in " + table);
                descs.Add(desc);
            }
            return b;
        }
        /// <summary> 
        /// Update or create a descriptor database table entry
        /// </summary>
        /// <param name="desc">The source descriptor instance</param>
        /// <returns>The unique DB id for the existing or newly created record</returns>
        public long Set(INCCDB.Descriptor desc)
        {
            if (!desc.modified)
                return -1;
            bool b = false;
            b = db.Update(desc.Name, desc.Desc);  // update or create in here, return long id
            long id = db.PrimaryKey(desc.Name);
            NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34032, "Updated/created " + table + " with {0} ({1})", desc.Name, id);
            return id;
        }

        public bool Delete(INCCDB.Descriptor desc)
        {
            bool b = false;
            if (db.Delete(desc.Name))
            {
                b = descs.Remove(desc);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34032, (b ? "Removed" : "Failed to remove") + desc.Name + " from " + table);
            }
            return b;
        }

        public bool Delete(string name)
        {
           INCCDB.Descriptor d = Get(name);
            return Delete(d);
        }
        public bool Delete(List<INCCDB.Descriptor> vals)
        {
            bool res = false;
            foreach (INCCDB.Descriptor d in vals)
            {
                if (db.Delete(d.Name))
                    res = res && descs.Remove(d);
                else
                    res = false;
            }
            return res;
        }
        public long SetList(List<INCCDB.Descriptor> vals = null)
        {
            long res = -1;
            if (descs == null)
            {
                int count = GetList().Count;
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34031, "{0} " + table + " read initially from DB", count);
            }
            try
            {
                foreach (INCCDB.Descriptor d in descs)
                {
                    res = Set(d);
                }
            }
            catch (Exception e)
            {
                NC.App.Pest.logger.TraceEvent(LogLevels.Error, 34033, table + " update punted out.");
                NC.App.Pest.logger.TraceException(e, false);
            }
            return res;
        }
    }

    public class MBAs : DescListImpl
    {
        public MBAs() : base(DB.Pieces.MBAs, "mbas")
        {
            db = new DB.Descriptors(table);
        }
     }
    public class Facilities : DescListImpl
    {
        public Facilities()
            : base(DB.Pieces.Facilities, "facility_names")
        {
            db = new DB.Descriptors(table);
        }
    }
    public class Materials : DescListImpl
    {
        public Materials()
            : base(DB.Pieces.Materials, "material_types")
        {
            db = new DB.Descriptors(table);
        }
    }
    public class DetectorTypes // : DescListImpl
    {
        public DetectorTypes()
           // : base(DB.Pieces.DetectorTypes, "detector_types")
        {
           // db = new DB.Descriptors(table);
        }

        /// INCC5 merges every known SR type into a short list of 6, to match sr lib
        /// This function adds the JSR-15 to the list
        /// MSR4 or 2150, JSR-11, JSR-12, PSR or ISR, DGSR, AMSR, JSR-15
        /// MSR4          JSR11   JSR12   PSR         DGSR  AMSR   JSR15
        
        public List<INCCDB.Descriptor> GetINCC5SRList()
        {
            if (mlimited == null)
            {
                mlimited = new List<INCCDB.Descriptor>();
                INCCDB.Descriptor d = new INCCDB.Descriptor("MSR4 or 2150", "MSR4A"); mlimited.Add(d);
                d = new INCCDB.Descriptor("JSR-11", "JSR11"); mlimited.Add(d);
                d = new INCCDB.Descriptor("JSR-12", "JSR12"); mlimited.Add(d);
                d = new INCCDB.Descriptor("PSR or ISR", "PSR"); mlimited.Add(d);
                d = new INCCDB.Descriptor("DGSR", "DGSR"); mlimited.Add(d);
                d = new INCCDB.Descriptor("AMSR", "AMSR"); mlimited.Add(d);
                d = new INCCDB.Descriptor("JSR-15", "JSR15"); mlimited.Add(d);
            }
            return mlimited;
        }

        public List<INCCDB.Descriptor> GetLMList()
        {
            if (lmlimited == null)
            {
                lmlimited = new List<INCCDB.Descriptor>();
                INCCDB.Descriptor d = new INCCDB.Descriptor("LMMM", "LMMM (LANL)"); lmlimited.Add(d);
                d = new INCCDB.Descriptor("PTR32", "PTR-32"); lmlimited.Add(d);
                d = new INCCDB.Descriptor("MCA527", "GBS Elektronik GmbH"); lmlimited.Add(d);
             }
            return lmlimited;
        }
        protected List<INCCDB.Descriptor> mlimited = null;
        protected List<INCCDB.Descriptor> lmlimited = null;

        public INCCDB.Descriptor Get()
        {
            GetINCC5SRList();
            GetLMList();
            return null;
        }

    }
    public class IOCodes : DescListImpl
    {
        public IOCodes()
            : base(DB.Pieces.IOCodes, "io_code")
        {
            db = new DB.Descriptors(table);
        }
    }
    public class InvChangeCodes : DescListImpl
    {
        public InvChangeCodes()
            : base(DB.Pieces.InvChangeCodes, "inventory_change_code")
        {
            db = new DB.Descriptors(table);
        }
    }

    /// <summary>
    /// In-memory and DB access descriptor list for stratums (strata?). Does not include historical stat fields
    /// </summary>
    public class Stratums : DescListImpl
    {
        public Stratums()
            : base(DB.Pieces.Strata, "stratum_ids")
        {
            db = new DB.Descriptors(table);
        }
    }

    /*
     * 
     *   
     * */

    /// <summary>
    /// Root of access to basic INCC persistent storage, tries to hide DB details LOL
    /// </summary>
    public partial class INCCDB
    {
        public INCCDB()
        {
            // set up the internal DB transfer state
            itemlist = new ItemIdListImpl();
            collarItemlist = new CollarItemIdListImpl();
            isotopics = new IsotopicsListImpl();
            comp_isotopics = new CompositeIsotopicsListImpl();
            testparams = new TestParamsImpl();

            mbas = new MBAs();
            facs = new Facilities();
            mats = new Materials();
            iocodes = new IOCodes();
            invchangecodes = new InvChangeCodes();
            dettypes = new DetectorTypes();
            stratums = new Stratums();

            normParameters = new NormParamsImpl();
            unattParameters = new UnattendedParamsImpl();
            backgroundParameters = new BackgroundParamsImpl();
            aasParameters = new AASParamsImpl();
            HVParameters = new HVParamsImpl();
            cm_pu_ratioparams = new cm_pu_ratio_Impl();
        }

        public static string MakeFrag(bool good)
        {
            return good ? "Created" : "Failed to create";
        }

        public static string UpdateIdFrag(long id)
        {
            return id < 0 ? "Failed to update " : ("Updated id " + id.ToString());
        }
        public static string MakeIdFrag(long id)
        {
            return id < 0 ? "Failed to create " : ("Created id " + id.ToString());
        }

        public static string UpdateFrag(bool good)
        {
            return good ? "Updated" : "Failed to update";
        }
        // optionally implement an initial data load here,
        // default is pieces are loaded as needed by calls through this class      
        public void Populate(DB.Persistence pest)
        {
        }

        // detectors (alpha/beta should attach to these but is on MultRes for now)
        DetectorList detectors;

        // parameters indirectly associated with detectors
        Dictionary<AcquireSelector, AcquireParameters> acqParameters;

        public static Detector GetDetectorParmsFromDataRow(DataRow dr, bool resultsSubset = false)
        {
            DetectorDefs.DataSourceIdentifier did = new DetectorDefs.DataSourceIdentifier();
            did.DetectorName = (string)dr["detector_name"];
            did.ElectronicsId = (string)(dr["electronics_id"]);
            did.Type = (string)(dr["detector_type_freeform"]);
            did.SRType = (DetectorDefs.InstrType)((Int32)(dr["detector_type_id"])); // todo: get AB, do a data table merge internally in the Pest Get method, like with bkg 
            if (!resultsSubset)
            { 
                did.ConnInfo = dr["sr_port_number"].ToString();
                did.BaudRate = DB.Utils.DBInt32(dr["sr_baud"]);
            }
            ShiftRegisterParameters sr = new ShiftRegisterParameters();
            sr.predelay = DB.Utils.DBUInt64(dr["predelay"]);
            sr.deadTimeCoefficientTinNanoSecs = DB.Utils.DBDouble(dr["multiplicity_deadtime"]);
            sr.deadTimeCoefficientAinMicroSecs = DB.Utils.DBDouble(dr["coeff_a_deadtime"]);
            sr.deadTimeCoefficientBinPicoSecs = DB.Utils.DBDouble(dr["coeff_b_deadtime"]);
            sr.deadTimeCoefficientCinNanoSecs = DB.Utils.DBDouble(dr["coeff_c_deadtime"]);
            sr.gateLength = DB.Utils.DBUInt64(dr["gate_length"]);
            sr.highVoltage = DB.Utils.DBDouble(dr["high_voltage"]);
            sr.dieAwayTime = DB.Utils.DBDouble(dr["die_away_time"]);
            sr.efficiency = DB.Utils.DBDouble(dr["efficiency"]);
            sr.doublesGateFraction = DB.Utils.DBDouble(dr["doubles_gate_fraction"]);
            sr.triplesGateFraction = DB.Utils.DBDouble(dr["triples_gate_fraction"]);

            AlphaBeta αβ = new AlphaBeta();
            if (!resultsSubset) 
            {
                // these two statements allocate the memory for each array
                αβ.α = DB.Utils.ReifyDoubles((string)dr["alpha_array"]);
                αβ.β = DB.Utils.ReifyDoubles((string)dr["beta_array"]);
            } // else // not preserved on results record, just init it

            Multiplicity mkey = new Multiplicity(DetectorDefs.InstrTypeExtensions.DefaultFAFor(did.SRType));
            mkey.SR = sr;

            Detector d = new Detector(did, mkey, αβ);

            return d;

        }


        public DetectorList Detectors
        {
            get
            {
                if (detectors == null)
                {
                    {
                        DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.Detectors);
                        detectors = new   DetectorList();
                        foreach (DataRow dr in dt.Rows)
                        {

                            Detector det = GetDetectorParmsFromDataRow(dr);
                            //if (αβ.Unset)
                            //{
                            //    MultiplicityCountingRes mcr = new MultiplicityCountingRes(mkey.FA, 0);
                            //    // This was not hitting on runs beyond the first one, and AB values were not set.  Put in INCCResults instead. hn 3.12.2015
                            //    INCCCycleConditioning.calc_alpha_beta(mkey, mcr);
                            //    det.AB.TransferIntermediates(mcr.AB);
                            //}

                            detectors.Add(det);
                        }
                    } // scope closure as a test

                    foreach (Detector det in detectors)
                    {
                        if (det.ListMode)
                        {
                            LMConnectionInfo lm = new LMConnectionInfo();
                            DataTable lmdt = NC.App.Pest.GetACollection(DB.Pieces.LMParams, det.Id.DetectorName);

                            if (lmdt.Rows.Count > 0)
                            {
                                DataRow drl = lmdt.Rows[0];
                                lm.DeviceConfig.LEDs = DB.Utils.DBInt32(drl["leds"]);
                                lm.DeviceConfig.HV = DB.Utils.DBInt32(drl["hv"]);
                                lm.DeviceConfig.LLD = DB.Utils.DBInt32(drl["LLD"]); // alias for VoltageTolerance on PTR32 and MCA527
                                lm.DeviceConfig.Debug = DB.Utils.DBInt32(drl["debug"]);
                                lm.DeviceConfig.Input = DB.Utils.DBInt32(drl["input"]);
                                try
                                {
                                    lm.DeviceConfig.HVTimeout = DB.Utils.DBInt32(drl["hvtimeout"]);
                                }
                                catch (Exception) { }
                                lm.NetComm.Broadcast = DB.Utils.DBBool(drl["broadcast"]);
                                lm.NetComm.LMListeningPort = DB.Utils.DBInt32(drl["broadcastport"]);
                                lm.NetComm.Port = DB.Utils.DBInt32(drl["port"]);
                                lm.NetComm.Subnet = (string)(drl["subnet"]);
                                lm.NetComm.Wait = DB.Utils.DBInt32(drl["wait"]);

                                lm.NetComm.NumConnections = DB.Utils.DBInt32(drl["numConnections"]);
                                lm.NetComm.ReceiveBufferSize = DB.Utils.DBInt32(drl["receiveBufferSize"]);
                                lm.NetComm.ParseBufferSize = DB.Utils.DBUInt32(drl["parseBufferSize"]);
                                lm.NetComm.UseAsynchAnalysis = DB.Utils.DBBool(drl["useAsyncAnalysis"]);
                                lm.NetComm.UseAsynchFileIO = DB.Utils.DBBool(drl["useAsyncFileIO"]);
                                lm.NetComm.UsingStreamRawAnalysis = DB.Utils.DBBool(drl["streamRawAnalysis"]);

                                // These values are used for each virtual SR analysis when doing an INCC or LM calc
                                // For each virtual SR used, the values are pulled from the LMMultiplicity table at the point the analysis occurs. 
                                // The table is populated using the Step 4 analysis wizard code
                                // DataTable lmmultdt = NC.App.Pest.GetACollection(DB.Pieces.LMMultParams, did.DetectorName);
                                //if (lmmultdt.Rows.Count > 0)
                                //{
                                //    DataRow dr2 = lmmultdt.Rows[0];
                                //    mkey.BackgroundGateTimeStepInTics = DB.Utils.DBUInt64(dr2["backgroundgatewidth"]);
                                //    mkey.AccidentalsGateDelayInTics = DB.Utils.DBUInt64(dr2["accidentalsgatewidth"]);
                                //    mkey.FA = (DB.Utils.DBBool(dr2["FA"]) ? FAType.FAOn : FAType.FAOff);
                                //    mkey.gateWidthTics = DB.Utils.DBUInt64(dr2["gatewidth"]);
                                //}

                            }
                            det.Id.FullConnInfo = lm;
                        }

                    }

                }
                return detectors;
            }
        }

        public static AcquireParameters GetAcquireParmsFromDataRow(ref string det, DataRow dr, bool resultsSubset, bool isLM)
        {
            AcquireParameters ap = new AcquireParameters();

            ap.facility = new Descriptor(dr["facility"].ToString(), dr["facility_description"].ToString());
            ap.mba = new Descriptor(dr["mba"].ToString(), dr["mba_description"].ToString());
            ap.item_type = dr["item_type"].ToString();
            ap.campaign_id = dr["campaign_id"].ToString();
            ap.item_id = dr["item_id"].ToString(); // same id in results_rec and acquire_parms_rec after all
            ap.stratum_id = new Descriptor(dr["stratum_id"].ToString(), dr["stratum_id_description"].ToString());
            ap.collar_mode = DB.Utils.DBBool(dr["collar_mode"].ToString());
            ap.inventory_change_code = dr["inventory_change_code"].ToString();
            ap.io_code = dr["io_code"].ToString();
            ap.well_config = (WellConfiguration)(DB.Utils.DBInt32(dr["well_config"].ToString()));
            ap.data_src = (DetectorDefs.ConstructedSource)(DB.Utils.DBInt32(dr["data_src"]));
            ap.qc_tests = DB.Utils.DBBool(dr["qc_tests"]);
            ap.error_calc_method = (ErrorCalculationTechnique)(DB.Utils.DBInt32(dr["error_calc_method"].ToString()));
            ap.print = DB.Utils.DBBool(dr["acq_print"].ToString());
            ap.user_id = dr["user_id"].ToString();
            ap.comment = dr["comment"].ToString();
            ap.num_runs = DB.Utils.DBUInt16(dr["num_runs"].ToString());
            if (resultsSubset)
                ap.detector_id = dr["detector_name"].ToString();
            else
                ap.detector_id = dr["meas_detector_id"].ToString();
            det = ap.detector_id;

            if (resultsSubset) return ap;

            ap.active_num_runs = DB.Utils.DBUInt16(dr["active_num_runs"].ToString());
            ap.facility = new Descriptor(dr["facility"].ToString(), dr["facility_description"].ToString());
            ap.mba = new Descriptor(dr["mba"].ToString(), dr["mba_description"].ToString());
            ap.detector_id = dr["meas_detector_id"].ToString();
            ap.glovebox_id = dr["glovebox_id"].ToString();
            ap.isotopics_id = dr["isotopics_id"].ToString();
            ap.comp_isotopics_id = dr["comp_isotopics_id"].ToString();

            ap.review_detector_parms = DB.Utils.DBBool(dr["review_detector_parms"]);
            ap.review_calib_parms = DB.Utils.DBBool(dr["review_calib_parms"]);
            ap.review_isotopics = DB.Utils.DBBool(dr["review_isotopics"]);
            ap.review_summed_raw_data = DB.Utils.DBBool(dr["review_summed_raw_data"]);
            ap.review_run_rate_data = DB.Utils.DBBool(dr["review_run_rate_data"].ToString());
            ap.review_run_raw_data = DB.Utils.DBBool(dr["review_run_raw_data"].ToString());
            ap.review_summed_mult_dist = DB.Utils.DBBool(dr["review_summed_mult_dist"].ToString());
            ap.review_run_mult_dist = DB.Utils.DBBool(dr["review_run_mult_dist"].ToString());

            ap.run_count_time = DB.Utils.DBDouble(dr["run_count_time"].ToString());
            ap.acquire_type = (AcquireConvergence)(DB.Utils.DBInt32(dr["acquire_type"].ToString()));

            ap.active_num_runs = DB.Utils.DBUInt16(dr["active_num_runs"].ToString());
            ap.max_num_runs = DB.Utils.DBUInt16(dr["max_num_runs"].ToString());
            ap.min_num_runs = DB.Utils.DBUInt16(dr["min_num_runs"].ToString());
            ap.meas_precision = DB.Utils.DBDouble(dr["meas_precision"].ToString());

            ap.mass = DB.Utils.DBDouble(dr["mass"].ToString());

            ap.drum_empty_weight = DB.Utils.DBDouble(dr["drum_empty_weight"].ToString());
            ap.MeasDateTime = DB.Utils.DBDateTimeOffset(dr["MeasDate"]);
            if (dr.Table.Columns.Contains("CheckDate"))
                ap.CheckDateTime = DB.Utils.DBDateTimeOffset(dr["CheckDate"]);
            ap.meas_detector_id = dr["meas_detector_id"].ToString();

            if (isLM)  // named detector is an LM, get the xtra params, unless they failed to get in DB
            {
                try
                {
                    ap.lm.Separation = DB.Utils.DBInt32(dr["separation"]);
                    ap.lm.Cycles = DB.Utils.DBInt32(dr["cycles"]);
                    ap.lm.Interval = DB.Utils.DBDouble(dr["interval"]);
                    ap.lm.MinHV = DB.Utils.DBInt32(dr["minHV"]);
                    ap.lm.MaxHV = DB.Utils.DBInt32(dr["maxHV"]);
                    ap.lm.Step = DB.Utils.DBInt32(dr["step"]);
                    ap.lm.HVDuration = DB.Utils.DBInt32(dr["hvduration"]);
                    ap.lm.Delay = DB.Utils.DBInt32(dr["delay"]);
                    ap.lm.HVX = DB.Utils.DBBool(dr["hvx"]);
                    ap.lm.Feedback = DB.Utils.DBBool(dr["feedback"]);
                    ap.lm.SaveOnTerminate = DB.Utils.DBBool(dr["saveOnTerminate"]);
                    if (!dr["results"].Equals(System.DBNull.Value))
                        ap.lm.Results = (string)(dr["results"]);
                    ap.lm.IncludeConfig = DB.Utils.DBBool(dr["includeConfig"]);
                    if (!dr["message"].Equals(System.DBNull.Value))
                        ap.lm.Message = (string)(dr["message"]);
                    ap.lm.LM = DB.Utils.DBInt32(dr["lm"]);
                    ap.lm.AssayType = DB.Utils.DBInt32(dr["AssayType"]);     // verify same as above    
					if (dr.Table.Columns.Contains("FADefault"))
						ap.lm.FADefault = (FAType)DB.Utils.DBInt32(dr["FADefault"]);
					else
						ap.lm.FADefault = FAType.FAOff; // new for 297

                }
                catch (ArgumentException)
                {
                }
                ap.lm.TimeStamp = ap.MeasDateTime;
             }
            return ap;
        }

        public static Stratum GetStratumByRow(DataRow dr, bool resultsSubset)
        {
            Stratum s = new Stratum();
            if (resultsSubset)
            {
                s.bias_uncertainty = DB.Utils.DBDouble(dr["bias_uncertainty"].ToString());
                s.random_uncertainty = DB.Utils.DBDouble(dr["random_uncertainty"].ToString());
                s.systematic_uncertainty = DB.Utils.DBDouble(dr["systematic_uncertainty"].ToString());
                s.relative_std_dev = DB.Utils.DBDouble(dr["relative_std_dev"].ToString());
            }
            else
            {
                s.bias_uncertainty = DB.Utils.DBDouble(dr["historical_bias"].ToString());
                s.random_uncertainty = DB.Utils.DBDouble(dr["historical_rand_uncert"].ToString());
                s.systematic_uncertainty = DB.Utils.DBDouble(dr["historical_systematic_uncert"].ToString());
                //Not in the Stratum record? HN
                //s.relative_std_dev = DB.Utils.DBDouble(dr["relative_std_dev"].ToString());
            }
            return s;
        }


        /// <summary>
        /// Get the current list of Stratums, (StratumDescriptor) including the nominal descriptor element and historic stat fields
        /// </summary>
        /// <param name="d">Optional detector to constrain list content</param>
        /// <returns>List of all stratums, or list of those associated with the specific detector</returns>
        public List<StratumDescriptor> StrataList(Detector d = null)
        {
            List<StratumDescriptor> stratii = new List<StratumDescriptor>();
            DataTable dt;
            if (d != null)
                dt = NC.App.Pest.GetACollection(DB.Pieces.StrataWithAssoc, d.Id.DetectorId);
            else
                dt = NC.App.Pest.GetACollection(DB.Pieces.Strata);                        
            
            foreach (DataRow dr in dt.Rows)
            {
                Descriptor sd = new Descriptor((string)dr["name"], (string)dr["description"]); 
                Stratum s = GetStratumByRow(dr,false);
                stratii.Add(new StratumDescriptor(sd, s));
            }
            return stratii;
        }



        public class AcquireSelector : Tuple<Detector, string, DateTimeOffset>
    {
            public AcquireSelector(Detector d, string itemtype, DateTimeOffset dt)
            : base(d, itemtype, dt) // might need a deep copy later
        {
        }

        public AcquireSelector()
                : base(null, string.Empty, DateTimeOffset.Now)
        {
        }

        public AcquireSelector(AcquireSelector src)
            : base(src.Item1, src.Item2, src.Item3)
        {
        }

        int Compare(AcquireSelector x, AcquireSelector y)
        {
            int res = 0;
           res = (new System.Collections.CaseInsensitiveComparer()).Compare(x.Item1, y.Item1);
           if (res == 0)
               res = x.Item2.CompareTo(y.Item2);
           if (res == 0)
               res = x.Item3.CompareTo(y.Item3);
           return res;
        }

        public int CompareTo(object other)
        {
            return Compare(this, (AcquireSelector)other);
        }
        public override bool Equals(object obj)
        {
            return (CompareTo(obj) == 0);
        }
       public override int GetHashCode()
        {
            int hCode = Item1.GetHashCode() ^ Item2.GetHashCode();
            return hCode;
        }
        public override string ToString()
        {
            return Item1          + "," + Item2 + " => " + Item3;
        }
        public Detector Detector 
        {
            get { return Item1; }
        }

        public DateTimeOffset TimeStamp
        {
            get { return this.Item3; }
        }

        public string ItemType
        {
            get { return this.Item2; }
        }
    }
    public AcquireParameters LastAcquire()
    {
         List<KeyValuePair<AcquireSelector, AcquireParameters>> l =   // this finds all acquire params, then sorts the saved params by insertion key
                    (from aq in NC.App.DB.AcquireParametersMap
                     orderby aq.Value.CheckDateTime descending
                     select aq).ToList();  // force eval
        if (l.Count > 0)
            return l.First().Value; // get the newest, it is the first on the sorted list
        else
            return new AcquireParameters();
    }

        public AcquireParameters LastAcquireFor(Detector d, string mtl_type)
        {
            List<KeyValuePair<AcquireSelector, AcquireParameters>> res =   // this finds the acquire params for the given detector, then sorts the params by date
                                    (from aq in NC.App.DB.AcquireParametersMap
                                     where (string.Equals(d.Id.DetectorId, aq.Value.detector_id) && string.Equals(mtl_type, aq.Value.item_type))
                                     orderby aq.Value.CheckDateTime descending
                                     select aq).ToList();  // force eval
            if (res.Count > 0)
                return res.First().Value;  // get the newest, it is the first on the sorted list
            else
                return null;
        }

		public AcquireParameters LastAcquireFor(Detector d)
        {
            List<KeyValuePair<AcquireSelector, AcquireParameters>> res =   // this finds the acquire params for the given detector, then sorts the params by date
                                    (from aq in NC.App.DB.AcquireParametersMap
                                     where string.Equals(d.Id.DetectorId, aq.Value.detector_id)
                                     orderby aq.Value.CheckDateTime descending
                                     select aq).ToList();  // force eval
            if (res.Count > 0)
                return res.First().Value;  // get the newest, it is the first on the sorted list
            else
                return null;
        }

        public Dictionary<AcquireSelector, AcquireParameters> AcquireParametersMap
        {
            get
            {
                if (acqParameters == null)
                {
                    acqParameters = new Dictionary<AcquireSelector, AcquireParameters>();
                    foreach (Detector d in Detectors)
                    {
                        DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.AcquireParams, d.Id.DetectorName);
                        foreach (DataRow dr in dt.Rows)
                        {
                            string det = d.Id.DetectorName;
                            AcquireParameters ap = GetAcquireParmsFromDataRow(ref det, dr, resultsSubset: false, isLM: d.ListMode);
                            AcquireSelector acs = new AcquireSelector(d, ap.item_type, ap.MeasDateTime);
                            if (!acqParameters.ContainsKey(acs))
                                acqParameters.Add(acs, ap);
                        }
                    }
                }
                return acqParameters;
            }
        }
        private Stratums stratums;
        public Stratums Stratums
        {
            get
            {
                stratums.Get();
                return stratums;
            }
        }
        private DetectorTypes dettypes;
        public DetectorTypes DetectorTypes
        {
            get
            {
                dettypes.Get();
                return dettypes;
            }
        }
        private InvChangeCodes invchangecodes;
        public InvChangeCodes InvChangeCodes
        {
            get
            {
                invchangecodes.Get();
                return invchangecodes;
            }
        }
        private IOCodes iocodes;
        public IOCodes IOCodes
        {
            get
            {
                iocodes.Get();
                return iocodes;
            }
        }

        private Materials mats;
        public Materials Materials
        {
            get
            {
                mats.Get();
                return mats;
            }
        }

        private MBAs mbas;
        public MBAs MBAs
        {
            get
            {
                mbas.Get();
                return mbas;
            }
        }

        private Facilities facs;
        public Facilities Facilities
        {
            get
            {
                facs.Get();
                return facs;
            }
        }
        private CollarItemIdListImpl collarItemlist;
        public CollarItemIdListImpl CollarItemIds
        {
            get
            {
                return collarItemlist;
            }
        }
        private ItemIdListImpl itemlist;
        public ItemIdListImpl ItemIds
        {
            get
            {
                return itemlist;
            }
        }
        private IsotopicsListImpl isotopics;
        public IsotopicsListImpl Isotopics
        {
            get
            {
                return isotopics;
            }
        }
        private CompositeIsotopicsListImpl comp_isotopics;
        public CompositeIsotopicsListImpl CompositeIsotopics
        {
            get
            {
                return comp_isotopics;
            }
        }
        private TestParamsImpl testparams;
        public TestParamsImpl TestParameters
        {
            get
            {
                return testparams;
            }
        }

        NormParamsImpl normParameters;
        public NormParamsImpl NormParameters
        {
            get
            {
                return normParameters;
            }
        }

        UnattendedParamsImpl unattParameters;
        public UnattendedParamsImpl UnattendedParameters
        {
            get
            {
                return unattParameters;
            }
        }

        BackgroundParamsImpl backgroundParameters;
        public BackgroundParamsImpl BackgroundParameters
        {
            get
            {
                return backgroundParameters;
            }
        }

        AASParamsImpl aasParameters;
        public AASParamsImpl AASSParameters
        {
            get
            {
                return aasParameters;
            }
        }

        private cm_pu_ratio_Impl cm_pu_ratioparams;
        public cm_pu_ratio_Impl Cm_Pu_RatioParameters
        {
            get
            {
                return cm_pu_ratioparams;
            }
        }

        public HVParamsImpl HVParameters { get; set; }

        /// <summary>
        /// Represents named items in INCC with descriptions
        /// All string and INCCDB.Descriptor comparisons are case-insensitive
        /// </summary>
        public class Descriptor : Tuple<string, string>, IComparer<Descriptor>
        {
            public Descriptor(string id, string desc)
                : base(id, desc) // might need a deep copy later
            {
            }

            public Descriptor()
                : base(string.Empty, string.Empty)
            {
            }

            public Descriptor(Descriptor src)
                : base(string.Copy(src.Item1), string.Copy(src.Item2))
            {
            }

            
            /// <summary>
            /// compares name property of x and y only
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns>true if Name is case-insensitively equal</returns>
            public int Compare(Descriptor x, Descriptor y)
            {
                return string.Compare(x.Item1, y.Item1, true);
            }

            /// <summary>
            /// compares name and desc properties of x and y 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns>true if both properties are case-insensitively equal</returns>
            public static int Compare2(Descriptor x, Descriptor y)
            {
              System.Collections.CaseInsensitiveComparer comp =   new System.Collections.CaseInsensitiveComparer();
              int n = comp.Compare(x.Item1, y.Item1);
              if (n == 0)
                  return comp.Compare(x.Item2, y.Item2);
              else
                  return n;
            }

            /// <summary>
            /// compares name and desc properties of this to other
            /// </summary>
            /// <param name="other"></param>
            /// <returns>true if both properties are case-insensitively equal</returns>
            public int CompareTo(Descriptor other)
            {
                return Compare2(this, other);
            }

            public string Name
            {
                get { return Item1; }
            }
            public string Desc
            {
                get { return Item2; }
            }

            public override string ToString()
            {
                return Item1;
            }
            public bool modified;
        }
                    
        /// <summary>
        /// Represents named stratums in INCC
        /// </summary>
        public class StratumDescriptor : Tuple<Descriptor, Stratum>
        {
            public StratumDescriptor( Descriptor desc, Stratum id) : base(desc, id) 
            {
            }

            public StratumDescriptor() : base(new Descriptor (), new Stratum())
            {
            }

            public StratumDescriptor(StratumDescriptor src)
                : base(new Descriptor(src.Item1), new Stratum(src.Item2))
            {
            }
            static int Compare(StratumDescriptor x, StratumDescriptor y)
            {
                int r = x.Item1.CompareTo(y.Item1);
                if (r == 0)
                    r = x.Item2.CompareTo(y.Item2);
                return r;
            }
            public int CompareTo(StratumDescriptor other)
            {
                return Compare(this, other);
            }
            public Descriptor Desc
            {
                get { return Item1; }
            }
            public Stratum Stratum
            {
                get { return Item2; }
            }
            public override string ToString()
            {
                return Item1.ToString();
            }
        }

        public class MeasurementResults : Tuple<MeasId, INCCResults.results_rec>
        {
            public MeasurementResults(MeasId id, INCCResults.results_rec res)
                : base(id, res)
            {
            }

            public MeasurementResults()
                : base(null, null)
            {
            }

            public MeasurementResults(MeasurementResults src)
                : base(new MeasId(src.Item1), new INCCResults.results_rec(src.Item2))
            {
            }

        }

        ///
        public List<MeasId> MeasurementIds(string detectorName, string mtype)
        {
            AssaySelector.MeasurementOption option = AssaySelectorExtensions.SrcToEnum(mtype);
            List<MeasId> foo;

            foo = new List<MeasId>();
            DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.Measurements, did:detectorName);
            foreach (DataRow dr in dt.Rows)
            {
                AssaySelector.MeasurementOption thisopt;
                if (option != AssaySelector.MeasurementOption.unspecified)
                {
                    thisopt = AssaySelectorExtensions.SrcToEnum(dr["Type"].ToString());
                    if (thisopt != option)
                        continue; // skip this one
                }
                // create the measurement id from the measurements table
                MeasId MeaId = new MeasId(
                    AssaySelectorExtensions.SrcToEnum(dr["Type"].ToString()),
                    DB.Utils.DBDateTimeOffset(dr["DateTime"]),
                    dr["FileName"].ToString(), DB.Utils.DBInt32(dr["id"])); // db table key actually
                foo.Add(MeaId);
            }

            if (foo.Count > 0)
            {
                ResultsRecs recs = new ResultsRecs();

                // get the traditional results rec that matches the measurement id 
                foreach (MeasId mid in foo)
                {
                    INCCResults.results_rec rec = recs.Get(mid.UniqueId); 
                    if (rec == null)
                        continue;
                    // To support UI operations on measurement lists, copy the item id from the results into the measurement id
                    mid.Item.Copy(rec.item);
                }
            }
            return foo;            
        }

 
        public List<Measurement> MeasurementsFor(Detector det, string MeasType = "")
        {
            List<Measurement> ms = new List<Measurement>();
            DataTable dt_meas;
            AssaySelector.MeasurementOption option = AssaySelectorExtensions.SrcToEnum(MeasType);
            ResultsRecs recs = new ResultsRecs();

			dt_meas = NC.App.Pest.GetACollection(DB.Pieces.Measurements, det.Id.DetectorName);

            foreach (DataRow dr in dt_meas.Rows)
            {
                AssaySelector.MeasurementOption thisopt;
                if (option != AssaySelector.MeasurementOption.unspecified)
                {
                    thisopt = AssaySelectorExtensions.SrcToEnum(dr["Type"].ToString());
                    if (thisopt != option)
                        continue; // skip this one, better to do this at the select level because the resultrecs above are relatively big objects to process 
                }

                // create the measurement id from the measurements table, augment with item id later
                MeasId MeaId = new MeasId(
                    AssaySelectorExtensions.SrcToEnum(dr["Type"].ToString()),
                    DB.Utils.DBDateTimeOffset(dr["DateTime"]),
                    dr["FileName"].ToString(), DB.Utils.DBInt32(dr["id"])); // db table key actually

                // get the traditional results rec that matches the measurement id 
                //This does not, in fact, get an item id......hn 9.10.2015
                INCCResults.results_rec rec = recs.Get(MeaId.UniqueId); //resultsList.Find(d => d.MeasurementIdMatch(MeaId));
                
                if (rec != null)
                { 
                    Measurement m = new Measurement(rec, MeaId, NC.App.Pest.logger);
                    MeaId.Item.Copy(rec.item);
                    ms.Add(m);
                    if (m.ResultsFiles != null)
					{
						if (!string.IsNullOrEmpty(dr["FileName"].ToString()))
						   m.ResultsFiles.Add (option == AssaySelector.MeasurementOption.unspecified, dr["FileName"].ToString());
						List<ResultFile> lrf = NC.App.DB.GetResultFiles(MeaId);
						foreach (ResultFile rf in lrf)
							m.ResultsFiles.Add(rf);
					}
                }
                // TODO: not needed by current UI caller, but needed for Reanalysis: cycles, results, method results, method params, etc 
            }

            return ms;
        }

		List<ResultFile> GetResultFiles(MeasId id)
		{
			List<ResultFile> l = new List<ResultFile> ();

            DB.Measurements ms = new DB.Measurements();
            DataTable dt = ms.GetResultFiles(id.UniqueId);  // this specific measurement id's results files
            foreach (DataRow dr in dt.Rows)
            {
				l.Add(new ResultFile(dr["FileName"].ToString()));
			}
			return l;
		}

        /// <summary>
        /// Construxt the CycleList from a stored measurement identified by the detector and the MeasId
        /// No LM data yet
        /// </summary>
        /// <param name="det">Detector</param>
        /// <param name="id">Measurement Id</param>
        /// <returns>CycleList</returns>
        public CycleList GetCycles(Detector det, MeasId mid)
        {
            CycleList cl = new CycleList();
			if (mid.UniqueId <= 0)
				return cl;

            DB.Measurements ms = new DB.Measurements();
            DataTable dt = null;
            dt = ms.GetCycles(mid.UniqueId);  // this specific measurement id's cycles
            int seq = 0;
            foreach (DataRow dr in dt.Rows)
            {
                seq++;
                Cycle c = new Cycle(NC.App.Pest.logger);
                cl.Add(c);
                c.TS = DB.Utils.DBTimeSpan(dr["cycle_time"]);
                c.Totals = DB.Utils.DBUInt64(dr["singles"]);
                c.HighVoltage = DB.Utils.DBDouble(dr["high_voltage"]);
                c.SinglesRate = DB.Utils.DBDouble(dr["singles_rate"]);
                c.seq = seq;
                if (dr.Table.Columns.Contains("chnhits") && (!dr["chnhits"].Equals(System.DBNull.Value)))
				{
					double[] att =  DB.Utils.ReifyDoubles((string)dr["chnhits"]);
					if (att.Length > 0)  // there was something there, use it
						c.HitsPerChannel = att;
					else
						c.HitsPerChannel[0] = c.Totals;
				}
				else
				{
	                c.HitsPerChannel[0] = c.Totals;
				}

                c.SetQCStatus(det.MultiplicityParams, (QCTestStatus)DB.Utils.DBInt32(dr["status"]), c.HighVoltage);
                MultiplicityCountingRes mcr = new MultiplicityCountingRes(det.MultiplicityParams.FA, 0);
                mcr.Scaler1 = VTuple.Create(DB.Utils.DBDouble(dr["scaler1"]), 0);
                mcr.Scaler2 = VTuple.Create(DB.Utils.DBDouble(dr["scaler2"]), 0);
                mcr.RASum = DB.Utils.DBDouble(dr["reals_plus_acc"]);
                mcr.ASum = DB.Utils.DBDouble(dr["acc"]);
                mcr.Mass = DB.Utils.DBDouble(dr["mass"]);
                mcr.rates[RatesAdjustments.Raw].Doubles = VTuple.Create(DB.Utils.DBDouble(dr["doubles_rate"]), 0);
                mcr.rates[RatesAdjustments.Raw].Triples = VTuple.Create(DB.Utils.DBDouble(dr["triples_rate"]), 0);
                mcr.rates[RatesAdjustments.Raw].Triples = VTuple.Create(DB.Utils.DBDouble(dr["triples_rate"]), 0);
                mcr.rates[RatesAdjustments.Raw].Scaler1s = VTuple.Create(DB.Utils.DBDouble(dr["scaler1_rate"]), 0);
                mcr.rates[RatesAdjustments.Raw].Scaler2s = VTuple.Create(DB.Utils.DBDouble(dr["scaler2_rate"]), 0);
                mcr.multiAlpha = DB.Utils.DBDouble(dr["multiplicity_alpha"]);
                mcr.efficiency = DB.Utils.DBDouble(dr["multiplicity_efficiency"]);
                mcr.multiplication = DB.Utils.DBDouble(dr["multiplicity_mult"]);
                mcr.RAMult = DB.Utils.ReifyUInt64s(dr["mult_reals_plus_acc"].ToString());
                mcr.NormedAMult = DB.Utils.ReifyUInt64s(dr["mult_acc"].ToString());

                mcr.UnAMult = new ulong[Math.Max(mcr.RAMult.Length, mcr.NormedAMult.Length)];
                mcr.Totals = c.Totals; // ??
                mcr.TS = new TimeSpan(c.TS.Ticks);
                mcr.RawSinglesRate.v = c.SinglesRate;
                // todo: this must happen eventually CycleProcessing.calc_alpha_beta(det.MultiplicityParams, mcr);

                c.CountingAnalysisResults.Add(det.MultiplicityParams, mcr);
            }

            return cl;
        }

        public bool AddCycles(CycleList cl, Detector det, Measurement m)
        {
            DB.Measurements ms = new DB.Measurements();
            long mid = m.MeasurementId.UniqueId;
            if (mid <= 0)
                return false;

            return AddCycles(cl, det.MultiplicityParams, mid, ms);
        }

        public bool AddCycles(CycleList cl, Multiplicity mkey, long mid, DB.Measurements db = null)
        {
            if (db == null)
                db = new DB.Measurements();
            int iCntCycles = cl.Count;
            List<DB.ElementList> clist = new List<DB.ElementList>();
            for (int ic = 0; ic < iCntCycles; ic++)
            {
                Cycle c = cl[ic];
                c.GenParamList(mkey); // URGENT: save results for EACH mkey (e.g. LM), not just the first one; save LM-specific cycle info, e.g. list mode channel results, per cycle counting results for raw LM analyses, output file name

                clist.Add(c.ToDBElementList(generate: false));
            }
            db.AddCycles(mid, clist);
            return true;
        }

        /// <summary>
        ///  short cut when DB id is known at creation time
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="c"></param>
        public void AddCycle(long mid, Cycle c, Multiplicity mkey)
        {
            DB.Measurements ms = new DB.Measurements();
            c.GenParamList(mkey); // URGENT: save results for EACH mkey (e.g. LM), not just the first one; save LM-specific cycle info, e.g. list mode channel results, per cycle counting results for raw LM analyses, output file name

            long lid = ms.AddCycleRetId(mid, c.ToDBElementList(generate: false));
        }
        // URGENT: design db tables for LM-specific results and implement parameter generator here (per cycle results)
        // invoke at the appropriate time from the single cycle with return value entry point

        public void AddResultsFileNames(Measurement m)
        {
            string primaryFilename = string.Empty; 
			bool skipTheFirstINCC5File = false;
			// Always use the first INCC5 filename for legacy consistency
            if (m.ResultsFiles != null && m.ResultsFiles.Count > 0) // need a defined filename and fully initialized Measurement here
			{
                primaryFilename = m.ResultsFiles.PrimaryINCC5Filename.Path;
				skipTheFirstINCC5File = true;
			}
            if (string.IsNullOrEmpty(primaryFilename))  // try the LM csv default name, this might be an LM measurement only
                primaryFilename = m.ResultsFiles.CSVResultsFileName.Path;
            if (string.IsNullOrEmpty(primaryFilename))  // only do the write if it's non-null
				return;

			DB.Measurements ms = new DB.Measurements();
            string type = m.MeasOption.ToString();
            long mid = m.MeasurementId.UniqueId;
			if (mid < 0) // no such measurement
				return;
            ms.UpdateFileName(primaryFilename, mid);

			// now add the remaining file names to the extended table
			List<ResultFile> rfl = new List<ResultFile>();
			if (skipTheFirstINCC5File && m.ResultsFiles.Count() > 1)
			{
				if (!string.IsNullOrEmpty(m.ResultsFiles.CSVResultsFileName.Path))
				{
					rfl.Add(m.ResultsFiles.CSVResultsFileName);
					rfl.AddRange(m.ResultsFiles.GetRange(1,m.ResultsFiles.Count() - 1));
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(m.ResultsFiles.CSVResultsFileName.Path))
				{
					rfl.Add(m.ResultsFiles.CSVResultsFileName);
					rfl.AddRange(m.ResultsFiles);
				}
			}
			if (rfl.Count() > 0)
				AddResultsFiles(rfl, mid, ms);
      
        }

		public bool AddAnalysisMessages(List<MeasurementMsg> msgs, Detector det, Measurement m)
        {
            DB.Measurements ms = new DB.Measurements();
            long mid = m.MeasurementId.UniqueId;
            if (mid <= 0)
                return false;

            return AddAnalysisMessages(msgs, mid, ms);
        }

        public bool AddAnalysisMessages(List<MeasurementMsg> msgs, long mid, DB.Measurements db = null)
        {
            if (db == null)
                db = new DB.Measurements();
            int imc = msgs.Count;
            List<DB.ElementList> mlist = new List<DB.ElementList>();
            for (int ic = 0; ic < imc; ic++)
            {
                MeasurementMsg c = msgs[ic];
                c.GenParamList();
                mlist.Add(c.ToDBElementList(generate: false));
            }
            db.AddMessages(mid, mlist);
            return true;
        }

		public bool AddResultsFiles(List<ResultFile> names, long mid, DB.Measurements db = null)
        {
            if (db == null)
                db = new DB.Measurements();
            int imc = names.Count;
            List<DB.ElementList> mlist = new List<DB.ElementList>();
            for (int ic = 0; ic < imc; ic++)
            {
                ResultFile c = names[ic];
                c.GenParamList();
                mlist.Add(c.ToDBElementList(generate: false));
            }
            db.AddResultsFiles(mid, mlist);
            return true;
        }

        public void UpdateAcquireParams(Detector det)
        {
            DB.AcquireParams aqdb = new DB.AcquireParams();
            AnalysisDefs.AcquireParameters acq = null;            
            var res =   // this finds the acquire params for the given detector and acquire type
                    from aq in AcquireParametersMap
                    where aq.Value.detector_id == det.Id.DetectorId
                    orderby aq.Value.MeasDateTime descending
                    select aq;
            foreach (KeyValuePair<AcquireSelector, AcquireParameters> kv in res)
            {
                acq = kv.Value;
                UpdateAcquireParams(acq, det.ListMode,aqdb);
            }
        }
        public void AddAcquireParams(AcquireSelector sel, AcquireParameters acq)
        {
            AcquireParametersMap.Add(sel, acq);
            UpdateAcquireParams(acq, sel.Detector.ListMode);
        }
        public void ReplaceAcquireParams(AcquireSelector sel, AcquireParameters acq)
        { 
            if (AcquireParametersMap.ContainsKey(sel))
            {
                AcquireParametersMap.Remove(sel);
            }
            AcquireParametersMap.Add(sel, acq);
            UpdateAcquireParams(acq, sel.Detector.ListMode);
        }

        public void UpdateAcquireParams(AcquireParameters acq, bool isLM = false, DB.AcquireParams aqdb = null)
        {
            if (aqdb == null)
                aqdb = new DB.AcquireParams();
            DB.ElementList saParams;
            DateTimeOffset pre = acq.CheckDateTime;     // devnote, internal timestamp always set here and only here
            acq.CheckDateTime = DateTimeOffset.Now;
            saParams = acq.ToDBElementList();
            bool ok = false;
            bool acqThere = aqdb.Has(acq.MeasDateTime, acq.detector_id, acq.item_type);
            if (!acqThere) // acq not there, so add it
            {
                ok = aqdb.Create(saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34011, MakeFrag(ok) + " new acquisition state {0} {1}", acq.MeasDateTime, acq.detector_id);
            }
            else
            {
                ok = aqdb.Update(acq.MeasDateTime, acq.detector_id, acq.item_type, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34011, UpdateFrag(ok) + " acquisition state for {0} {1}", acq.MeasDateTime, acq.detector_id);
            }
            if (ok)
                acq.modified = false;
            else // restore the check date, the acq record did not get saved
                acq.CheckDateTime = pre;

            if (isLM)
            {
                DB.LMAcquireParams lmaqdb = new DB.LMAcquireParams(aqdb.db);
                DB.ElementList els = acq.lm.ToDBElementList();
                els.Add(new DB.Element("item_type", acq.item_type)); // used for joins with the parent acq
                if (!lmaqdb.Has(acq.MeasDateTime, acq.detector_id, acq.item_type)) // item not there, so add it
                {
                    ok = lmaqdb.Create(acq.MeasDateTime, acq.detector_id, els);
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34038, MakeFrag(ok) + " LM acq state {0} {1}", acq.MeasDateTime, acq.detector_id);
                }
                else
                {
                    ok = lmaqdb.Update(acq.detector_id, acq.item_type, els);
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34039, UpdateFrag(ok) + " LM acq state for {0} {1}", acq.MeasDateTime, acq.detector_id);
                }
            }
        }

        public void UpdateDetectorαβ(Detector dr, DB.Detectors db = null)
        {
            if (db == null) db = new DB.Detectors();

            DB.ElementList αβparams = dr.AB.ToDBElementList();
            DB.AlphaBeta αβ = new DB.AlphaBeta(db.db);
            bool therehtere = αβ.Has(dr.Id.DetectorId);
            if (!therehtere) // not there, so add it
            {
                bool a = αβ.Create(dr.Id.DetectorId, αβparams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34009, MakeFrag(a) + " detector αβ for {0}", dr.Id.DetectorId);
            }
            else
            {
                bool a = αβ.Update(dr.Id.DetectorId, αβparams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34009, UpdateFrag(a) + " detector αβ for {0}", dr.Id.DetectorId);
            }
            // todo: then do multiplicity, if required, (study it)
        }

        /// <summary>
        /// Create or update the parameters associated with a Shift Register or List Mode detector.
        /// Invoke when a new detector is created, or when a change is made to an existing detector's parameters.
        /// Assumes the containing detector is successfully inserted in the the database detectors table, and has a valid unique key</summary>
        /// <param name="dr">The detector</param>
        /// <param name="db">Optional instance of the detector database connection class. If null, a new instance is created</param>
        public void UpdateDetectorParams(Detector dr, DB.Detectors db = null)
        {

            if (db == null) db = new DB.Detectors();

            DB.ElementList els = dr.SRParams.ToDBElementList();
            long l = db.PrimaryKey(dr.Id.DetectorId);
            if (l < 0)
            {
                NC.App.Pest.logger.TraceEvent(LogLevels.Warning, 34035, "No detector '{0}' found in the database, create it first", dr.Id.DetectorId);
                return;
            }
            els.Add(new DB.Element("detector_id", l));
            els.Add(new DB.Element("sr_detector_id", dr.Id.DetectorName));
            els.Add(new DB.Element("sr_type", (Int32)dr.Id.SRType));

            if (!dr.Id.SRType.IsListMode())
            {
                els.Add(dr.Id.NewForINCC6Params.AsElement);
                els.Add(new DB.Element("sr_port_number", dr.Id.SerialPort));
            }

            DB.ShiftRegisterParams sr = new DB.ShiftRegisterParams(db.db,newid:true);
            bool therehtere = sr.Has(dr.Id.DetectorId);
            if (!therehtere) // not there, so add it
            {
                bool a = sr.Create(dr.Id.DetectorId, (Int32)(dr.Id.SRType), els);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34009, MakeFrag(a) + " detector SR params for {0}", dr.Id.DetectorId);
            }
            else
            {
                // The containing detector must be defined in the database before an update operation can succeed. 
                // This fails when this record exist but was inserted without a parent detector record (id = -1)

                bool a = sr.Update(dr.Id.DetectorId, (Int32)(dr.Id.SRType), els);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34009, UpdateFrag(a) + " detector SR params for {0}", dr.Id.DetectorId);
            }
            if (dr.Id.SRType.IsListMode())
            {
                DB.LMNetCommParams lnnc = new DB.LMNetCommParams(db.db);
                LMConnectionInfo info = (LMConnectionInfo)dr.Id.FullConnInfo;
                DB.ElementList lmparams = info.NetComm.ToDBElementList();

                if (!lnnc.Has(dr.Id.DetectorId))
                {
                    long a = lnnc.CreateNetComm(l, lmparams, db.db);
                    a = lnnc.CreateCfg(l, info.DeviceConfig.ToDBElementList(), db.db);
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34034, MakeIdFrag(a) + " detector LM params for {0}", dr.Id.DetectorId);
                }
                else
                {
                    bool b = lnnc.UpdateNetComm(l, lmparams, db.db);
                    b = lnnc.UpdateCfg(l, info.DeviceConfig.ToDBElementList(), db.db);
                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34035, UpdateFrag(b) + " detector LM params for {0}", dr.Id.DetectorId);
                }
            }
            // todo: then multiplicity, but ... values may already be preserved for LMMM, and not needed for SR, analyze
        }


        public void UpdateDetector(Detector det, DB.Detectors db = null)
        {
            if (db == null) db = new DB.Detectors();
           
            DB.ElementList els = null;
            els = new DB.ElementList();
            els.Add(new DB.Element("detector_name",det.Id.DetectorName)); 
            
            if (!db.DefinitionExists(els))
            {
                long id = db.Insert(det.Id.DetectorId, det.Id.Identifier(), (Int32)det.Id.SRType, det.Id.ElectronicsId, det.Id.Type);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34013, MakeIdFrag(id) + " for detector {0}", det.Id.DetectorName);
            }
            else
            {
                DB.ElementList full = det.Id.ToDBElementList();
                full.AddRange(els);
                bool b = db.Update(det.Id.DetectorName, full);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34014, UpdateFrag(b) + " for detector {0}", det.Id.DetectorName);
            }

			UpdateDetectorParams(det, db);  // inserting the related sr_parms_rec maintains the complete detector and param record set in the database.
			UpdateDetectorαβ(det, db);
        }

		
        public bool UpdateDetectorFields(Detector det, DB.Detectors db = null)
        {
            if (db == null) db = new DB.Detectors();
           
            DB.ElementList els = null;
            els = new DB.ElementList();
            els.Add(new DB.Element("detector_name",det.Id.DetectorName)); 
            bool b = false;
            if (db.DefinitionExists(els))
            {
                DB.ElementList full = det.Id.ToDBElementList();
                full.AddRange(els);
                b = db.Update(det.Id.DetectorName, full);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34014, UpdateFrag(b) + " for detector {0}", det.Id.DetectorName);
            }
			return b;
        }

		        // update the accumulated changes on the global list
        public void UpdateDetectors()
        {
            DB.Detectors db = new DB.Detectors();
            SortedSet<Detector> ssd = new SortedSet<Detector>(Detectors);

            foreach (Detector dr in ssd)
            {
                UpdateDetector(dr, db);
                UpdateDetectorParams(dr, db);  // this is required to complete a new detector update from INCC5 initial data import 
            }
            NC.App.Pest.logger.TraceInformation("Detectors updated");
        }
        // todo: have added deletes for norm, unattended and sr, however since detector_id is a foreign key for EVERYTHING, DELETE call blows.
        public bool DeleteDetector(Detector det)
        {
            DB.Detectors dets = new DB.Detectors();
            if (dets.Delete(det.Id.DetectorName, det.Id.ElectronicsId, (Int32)(det.Id.SRType)))
            {
                string name = det.Id.DetectorName;
                bool b = Detectors.Remove(det);
                NC.App.Pest.logger.TraceEvent(LogLevels.Info, 34008, (b ? "Deleted detector {0}" : "Failed to delete {0}"), name);
                return b;
            }
            else
                return false;
        }


        public bool UpdateStratum(Descriptor d, Stratum s)
        {
            DB.Strata srt = new DB.Strata();
            DB.ElementList saParams;
            saParams = s.ToDBElementList();
            // update the name and description
            saParams[0].Value = d.Name;
            saParams[1].Value = d.Desc;
            bool there = srt.Has(d.Name);
            bool rsult = false;
            if (!there) // item not there, so add it
            {
                long l = srt.Create(saParams);
                rsult = (l > 0);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34010, MakeIdFrag(l) + " stratum {0}",  d.Name);
            }
            else
            {
                rsult = srt.Update(d.Name, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34010, UpdateFrag(rsult) + "  stratum {0}", d.Name);
            }
            return rsult;
        }
        public bool DeleteStratum(Descriptor d)  // todo: delete other params related -- hn doing checks somewhere else to ask user if they want to delete.  See IDStratumIdDelete
        {
            DB.Strata strata = new DB.Strata();

            return strata.Delete(d.Name);
        }

        public void AssociateStratum(Detector det, Descriptor d, Stratum s)
        {
            DB.Strata srt = new DB.Strata();
            DB.ElementList saParams;
            saParams = s.ToDBElementList();
            // update the name and description
            saParams[0].Value = d.Name;
            saParams[1].Value = d.Desc;
            bool stratthere = srt.Has(d.Name);
            bool there = srt.HasAssociation(det.Id.DetectorName, d.Name);
            if (!stratthere)
            {
                bool b = srt.Create(det.Id.DetectorName, saParams);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34012, MakeFrag(b) + " stratum and stratum detector association for {0} {1}", det.Id.DetectorName, d.Name);
            }
            else if (!there)
            {
                bool b = srt.Associate(det.Id.DetectorName, d.Name);
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34012, MakeFrag(b) + " stratum detector association for {0} {1}", det.Id.DetectorName, d.Name);
            }   
            
        }



        /// <summary>
        /// Update the stored state (theDB) with modifications on the collections in this class
        /// * resolve conflicts involving replacements and updates to each collection and contained object automatically (as much as possible)
        /// * some state is required to make these choices, e.g. 'overwrite', 'ask before overwrite' 'never overwrite'
        /// * go for the detector and it's cousins
        /// * finally do the measurement
        /// </summary>
        /// <returns></returns>
        public bool Udpate()
        {
            try
            {
                // after BuildMeasurement, these are already taken care of in-memory and in the DB
                // measurement
                // background
                // norm
                // test
                // acquire 
                // detectors
                // stratum                
                // isotopics
                // unattended
                // (Detector, Material) -> Analysis Methods
                // materials
                // MBAs
                // facilities

                // todo: needs DB persistence
                // finish all results, including Mult Results
                // 

                return true;
            }
            catch (Exception e)
            {
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                NC.App.Pest.logger.TraceException(e, false);
            }
            return false;
        }
    }
}

